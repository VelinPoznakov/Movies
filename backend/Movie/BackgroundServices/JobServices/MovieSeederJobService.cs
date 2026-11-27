using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.StaticFiles;
using Movie.BackgroundServices.Contracts;
using Movie.Dtos.TMDB.Cast;
using Movie.Dtos.TMDB.Genre;
using Movie.Dtos.TMDB.Movie;
using Movie.Dtos.TMDB.ProductionCompany;
using Movie.Entities;
using Movie.Entities.Enums;
using Movie.ExternalSystems.Contracts;
using Movie.Repositories.Interfaces;

namespace Movie.BackgroundServices.JobServices;

public class MovieSeederJobService: IScheduleTask
{
    private readonly ITMDBHandler _requestHandler;
    private readonly ILogger<MovieSeederJobService> _logger;
    private readonly string _operationName;
    private readonly IBlobHandler _blobHandler;
    private readonly IConfigurationSection _blobContainersSection;
    private readonly IConfigurationSection _tmdbSection;
    private readonly Stopwatch _stopwatch;
    private readonly IBaseRepository<MovieEntity> _movieRepository;
    private readonly IBaseRepository<Genre>  _genreRepository;
    private readonly IBaseRepository<ProducerCompany>  _producerCompanyRepository;
    private readonly IBaseRepository<Director>  _directorRepository;
    private readonly int AllowedRetryCount = 3;
         
    public MovieSeederJobService(
        ITMDBHandler requestHandler,
        ILogger<MovieSeederJobService> logger,
        IBlobHandler blobHandler,
        IConfiguration configuration,
        IBaseRepository<MovieEntity> movieRepository,
        IBaseRepository<Genre> genreRepository,
        IBaseRepository<ProducerCompany> producerCompanyRepository,
        IBaseRepository<Director> directorRepository)
    {
        _requestHandler = requestHandler;
        _logger = logger;
        _blobHandler = blobHandler;
        _blobContainersSection = configuration.GetSection("BlobStorage:Containers");
        _tmdbSection = configuration.GetSection("TMDBSettings");
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _producerCompanyRepository = producerCompanyRepository;
        _directorRepository = directorRepository;
        _stopwatch = new Stopwatch();
        _operationName = nameof(MovieSeederJobService);
    }
    
    public Task ExecuteAsync(CancellationToken stoppingToken) => SeedMoviesAsync();

    private const int TMDB_MAX_PAGES = 500;

    private async Task SeedMoviesAsync()
    {
        _stopwatch.Start();

        _logger.LogInformation($"{_operationName} started");

        DateOnly startDate = _tmdbSection.GetValue<DateOnly>("FromDate");
        DateOnly toDate = DateOnly.FromDateTime(DateTime.UtcNow);

        while (toDate >= startDate)
        {
            DateOnly fromDate = toDate.AddYears(-1).AddDays(1);

            if (fromDate < startDate)
                fromDate = startDate;

            await SeedPeriodAsync(fromDate, toDate);

            toDate = fromDate.AddDays(-1);
        }

        _stopwatch.Stop();

        _logger.LogInformation($"{_operationName} ran for {_stopwatch.Elapsed.TotalMinutes:F0} minutes");
    }

    private async Task SeedPeriodAsync(DateOnly fromDate, DateOnly toDate)
    {
        int totalPages = 1;
        int currentPage = 1;

        do
        {
            MovieCollectionResponseDto? moviesCollection =
                await _requestHandler.DiscoverMoviesAsync(fromDate, toDate, currentPage);

            if (moviesCollection == null)
            {
                _logger.LogInformation($"No movies found for {fromDate} - {toDate} on page {currentPage}");
                break;
            }

            foreach (MovieResponseDto movie in moviesCollection.Movies)
            {
                for (int attempt = 1; attempt <= AllowedRetryCount; attempt++)
                {
                    try
                    {
                        await SeedMovieAsync(movie.TmdbId);
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex, $"{movie.TmdbId} failed on attempt {attempt}.");
                        continue;
                    }
                    finally
                    {
                        _movieRepository.ClearTracking();
                    }
                }
            }
            
            if (moviesCollection.TotalPages > TMDB_MAX_PAGES)
                _logger.LogWarning($"{fromDate} - {toDate} has {moviesCollection.TotalPages} pages, only the first {TMDB_MAX_PAGES} are reachable");

            totalPages = Math.Min(moviesCollection.TotalPages, TMDB_MAX_PAGES);

            _logger.LogInformation($"{fromDate} - {toDate}: page {currentPage} of {totalPages} done");

            currentPage++;

        } while (currentPage <= totalPages);
    }

    private async Task SeedMovieAsync(long tmdbId)
    {
        if(await _movieRepository.DoesEntityExistByTmdbIdAsync(tmdbId))
            return;
        
        MovieDetailsResponseDto? movieDetailsResponseDto = await _requestHandler
            .GetMovieDetailsAsync(tmdbId, true);

        if (movieDetailsResponseDto == null)
        {
            _logger.LogInformation($"Movie Id {tmdbId} was not found in details");
            return;
        }

        Enum.TryParse(movieDetailsResponseDto.Status?.Replace(" ", ""), ignoreCase: true,
            out MovieStatus status);

        DateOnly? date = null;

        if (DateOnly.TryParseExact(movieDetailsResponseDto.ReleaseDate, "yyyy-MM-dd",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
            date = parsedDate;

        if (date == null)
        {
            _logger.LogInformation($"Movie Id {tmdbId} has no valid release date and is skipped");
            return;
        }

        MovieEntity newMovie = new MovieEntity()
        {
            TmdbId = movieDetailsResponseDto.TmdbId,
            Budget = movieDetailsResponseDto.Budget,
            Description = movieDetailsResponseDto.Description,
            ImdbId = movieDetailsResponseDto.ImdbId,
            IsAdult = movieDetailsResponseDto.IsAdult,
            TimeLong = movieDetailsResponseDto.Runtime,
            Title = movieDetailsResponseDto.Title,
            Status = status,
            PosterBlobName =  await CreateImageIntoBlobStorage(
                _blobContainersSection["posters"]!, _tmdbSection["PosterSize"]!, movieDetailsResponseDto.PosterPath),
            BackDroopPoster = await CreateImageIntoBlobStorage(
                _blobContainersSection["backdrops"]!, _tmdbSection["BackdropSize"]!, movieDetailsResponseDto.BackdropPath),
            ReleaseDate = date.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
        };

        foreach (GenreResponseDto genreResponseDto in movieDetailsResponseDto.GenreResponseDtos
                     .DistinctBy(g => g.TmdbId))
        {
            Genre? genre = await _genreRepository
                .GetEntityByIdAsync<Genre>(genreResponseDto.TmdbId, searchByTmdbId: true, asNoTracking: false);

            if (genre != null)
            {
                newMovie.MoviesGenres.Add(new MoviesGenres
                {
                    Genre = genre,
                });

                continue;
            }
            
            newMovie.MoviesGenres.Add(new MoviesGenres
            {
                Genre = new Genre
                {
                    TmdbId = genreResponseDto.TmdbId,
                    Name = genreResponseDto.Name
                }
            });
        }

        foreach (ProductionComapnyResponseDto companyResponseDto in movieDetailsResponseDto.ProductionCompanies
                     .DistinctBy(c => c.TmdbId))
        {
            ProducerCompany? producerCompany = await _producerCompanyRepository
                .GetEntityByIdAsync<ProducerCompany>(companyResponseDto.TmdbId, searchByTmdbId: true, asNoTracking: false);
            
            if (producerCompany != null)
            {
                newMovie.MoviesProducerCompanies.Add(new MoviesProducerCompanies()
                {
                    ProducerCompany =  producerCompany
                });

                if (producerCompany.LogoPath == null && companyResponseDto.LogoPath != null)
                {
                    producerCompany.LogoPath = await CreateImageIntoBlobStorage(
                        _blobContainersSection["CompanyProducer"]!, _tmdbSection["LogoSize"]!, 
                        companyResponseDto.LogoPath);
                }
                
                continue;
            }
            
            newMovie.MoviesProducerCompanies.Add(new MoviesProducerCompanies
            {
                ProducerCompany = new ProducerCompany
                {
                    TmdbId = companyResponseDto.TmdbId,
                    Name = companyResponseDto.Name,
                    LogoPath = await CreateImageIntoBlobStorage(
                        _blobContainersSection["CompanyProducer"]!, _tmdbSection["LogoSize"]!, companyResponseDto.LogoPath),
                }
            });
        }

        foreach (CrewDto member in movieDetailsResponseDto.CreditsDto.Crew
                     .Where(c => c.Job == "Director")
                     .DistinctBy(c => c.TmdbId))
        {
            Director? director = await _directorRepository
                .GetEntityByIdAsync<Director>(member.TmdbId, searchByTmdbId: true, asNoTracking: false);

            if (director != null)
            {
                newMovie.MoviesDirectors.Add(new MoviesDirectors
                {
                    Director = director,
                });

                if (director.ProfilePath == null && member.ProfilePath != null)
                {
                    director.ProfilePath = await CreateImageIntoBlobStorage(
                        _blobContainersSection["director"]!, _tmdbSection["ProfileSize"]!, 
                        member.ProfilePath);
                }
                
                continue;
            }
            
            newMovie.MoviesDirectors.Add(new MoviesDirectors
            {
                Director = new Director()
                {
                    TmdbId = member.TmdbId,
                    Name = member.Name,
                    ProfilePath = await CreateImageIntoBlobStorage(_blobContainersSection["director"]!, _tmdbSection["ProfileSize"]!,
                        member.ProfilePath)
                }
            });
        }
        
        await _movieRepository.CreateEntityAsync(newMovie);
    }

    // errors that can go away on their own; anything else fails the same way every time
    private static bool IsTransient(Exception ex) => ex switch
    {
        HttpRequestException => true,
        TaskCanceledException => true,
        TimeoutException => true,
        Npgsql.NpgsqlException { IsTransient: true } => true,
        Microsoft.EntityFrameworkCore.DbUpdateException { InnerException: Npgsql.NpgsqlException { IsTransient: true } } => true,
        Azure.RequestFailedException { Status: 408 or 429 or 500 or 502 or 503 or 504 } => true,
        _ => false
    };


    private async Task<string?> CreateImageIntoBlobStorage(string containerName, string size, string? tmdbImageFileName)
    {
        if (string.IsNullOrEmpty(tmdbImageFileName))
            return null;

        await using Stream? imageStream = await _requestHandler.GetImageStreamAsync($"{size}{tmdbImageFileName}");

        if (imageStream == null)
            return null;

        string formatedTmdbImageFileName = CreateImageBlobName(tmdbImageFileName);

        if (!new FileExtensionContentTypeProvider().TryGetContentType(tmdbImageFileName, out string? contentType))
            contentType = "application/octet-stream";

        string blobStorageResult = await _blobHandler.UploadAsync(containerName, formatedTmdbImageFileName,
            imageStream, contentType);

        return blobStorageResult;
    }

    private string CreateImageBlobName(string tmdbImageFileName)
    {
        string[] blobParts = tmdbImageFileName.Split('.');

        if (blobParts.Length < 2)
            return null;
        
        blobParts[0] = Guid.NewGuid().ToString();

        if (blobParts[0].Contains('/'))
            blobParts[0] = blobParts[0].Replace("/", "");

        return string.Join('.', blobParts);

    }
}