using System.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Movie.BackgroundServices.Contracts;
using Movie.Entities;
using Movie.ExternalSystems.Contracts;
using Movie.Repositories.Interfaces;

namespace Movie.BackgroundServices.JobServices;

public class MissingPicturesJobService: IScheduleTask
{
    private readonly IBaseRepository<MovieEntity>  _movieRepository;
    private readonly IBaseRepository<Director> _directorRepository;
    private readonly IBaseRepository<ProducerCompany> _producerCompanyRepository;
    private readonly ITMDBHandler _tmDBHandler;
    private readonly IConfigurationSection _configuration;
    private readonly ILogger<MissingPicturesJobService> _logger;
    private readonly string _operationName;
    private readonly IBlobHandler _blobHandler;
    private readonly IConfigurationSection _blobConfigurationSection;
    
    public MissingPicturesJobService(
        IBaseRepository<MovieEntity> movieRepository,
        IBaseRepository<Director> directorRepository,
        IBaseRepository<ProducerCompany> producerCompanyRepository,
        ITMDBHandler tmDBHandler,
        IConfiguration configuration,
        ILogger<MissingPicturesJobService> logger,
        IBlobHandler blobHandler)
    {
        _movieRepository = movieRepository;
        _directorRepository = directorRepository;
        _producerCompanyRepository = producerCompanyRepository;
        _tmDBHandler = tmDBHandler;
        _configuration = configuration.GetSection("TMDBSettings");
        _operationName = nameof(MissingPicturesJobService);
        _logger = logger;
        _blobHandler = blobHandler;
        _blobConfigurationSection = configuration.GetSection("BlobStorage:Containers");
    }
    
    public Task ExecuteAsync(CancellationToken stoppingToken) => HandleMissingPictures();
    
    private async Task HandleMissingPictures()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        _logger.LogInformation("{Operation} started", _operationName);

        List<MovieEntity> moviesWithoutPosterOrBackdrop = (await _movieRepository
            .GetAllAsync<MovieEntity>(
                filter: m => m.PosterBlobName == null
                             || m.BackDroopPoster == null))
            .ToList();

        List<Director> directorsWithoutPicture = (await _directorRepository
            .GetAllAsync<Director>(
                filter: d => d.ProfilePath == null))
            .ToList();

        List<ProducerCompany> producerCompaniesWithoutPicture = (await _producerCompanyRepository
            .GetAllAsync<ProducerCompany>(
                filter: pc => pc.LogoPath == null))
            .ToList();

        _logger.LogInformation(
            "{Operation} found {Movies} movies, {Directors} directors and {Companies} companies with missing images",
            _operationName, moviesWithoutPosterOrBackdrop.Count, directorsWithoutPicture.Count,
            producerCompaniesWithoutPicture.Count);

        await FillMissingImagesAsync(
            moviesWithoutPosterOrBackdrop.Where(m => m.PosterBlobName == null),
            async tmdbId => (await _tmDBHandler.GetMovieDetailsAsync(tmdbId))?.PosterPath,
            (movie, url) => movie.PosterBlobName = url,
            "Posters", "PosterSize", _movieRepository);

        await FillMissingImagesAsync(
            moviesWithoutPosterOrBackdrop.Where(m => m.BackDroopPoster == null),
            async tmdbId => (await _tmDBHandler.GetMovieDetailsAsync(tmdbId))?.BackdropPath,
            (movie, url) => movie.BackDroopPoster = url,
            "Backdrops", "BackdropSize", _movieRepository);

        await FillMissingImagesAsync(
            directorsWithoutPicture,
            async tmdbId => (await _tmDBHandler.GetPersonAsync(tmdbId))?.ProfilePath,
            (director, url) => director.ProfilePath = url,
            "Director", "ProfileSize", _directorRepository);

        await FillMissingImagesAsync(
            producerCompaniesWithoutPicture,
            async tmdbId => (await _tmDBHandler.GetCompanyAsync(tmdbId))?.LogoPath,
            (company, url) => company.LogoPath = url,
            "CompanyProducer", "LogoSize", _producerCompanyRepository);

        stopwatch.Stop();

        _logger.LogInformation("{Operation} finished in {Minutes:F1} minutes",
            _operationName, stopwatch.Elapsed.TotalMinutes);
    }

    private async Task FillMissingImagesAsync<T>(
        IEnumerable<T> entities,
        Func<long, Task<string?>> getTmdbImagePath,
        Action<T, string> setImageUrl,
        string containerKey,
        string sizeKey,
        IBaseRepository<T> repository)
        where T : BaseTMDB
    {
        string typeName = typeof(T).Name;
        List<T> entityList = entities.ToList();
        List<T> updated = new List<T>();
        int noTmdbId = 0;
        int noImageOnTmdb = 0;
        int failed = 0;
        int processed = 0;

        _logger.LogInformation("{Type} {Container}: checking {Total} entities", typeName, containerKey, entityList.Count);

        foreach (T entity in entityList)
        {
            processed++;

            if (processed % 100 == 0)
                _logger.LogInformation("{Type} {Container}: {Processed} of {Total} checked, {Updated} filled so far",
                    typeName, containerKey, processed, entityList.Count, updated.Count);

            if (entity.TmdbId is not long tmdbId)
            {
                noTmdbId++;
                _logger.LogDebug("{Type} {Id} skipped: no TmdbId", typeName, entity.Id);
                continue;
            }

            try
            {
                string? tmdbImagePath = await getTmdbImagePath(tmdbId);

                if (tmdbImagePath == null)
                {
                    noImageOnTmdb++;
                    _logger.LogDebug("{Type} {Id} (TMDB {TmdbId}): TMDB has no image", typeName, entity.Id, tmdbId);
                    continue;
                }

                string? imageUrl = await CreateImageIntoBlobStorage(
                    _blobConfigurationSection[containerKey]!, _configuration[sizeKey]!, tmdbImagePath);

                if (imageUrl == null)
                {
                    failed++;
                    _logger.LogWarning("{Type} {Id} (TMDB {TmdbId}): image {Path} could not be stored",
                        typeName, entity.Id, tmdbId, tmdbImagePath);
                    continue;
                }

                setImageUrl(entity, imageUrl);
                updated.Add(entity);

                _logger.LogDebug("{Type} {Id} (TMDB {TmdbId}): stored {Url}", typeName, entity.Id, tmdbId, imageUrl);
            }
            catch (Exception ex)
            {
                failed++;
                _logger.LogWarning(ex, "{Type} {Id} (TMDB {TmdbId}): failed to fill {Container}",
                    typeName, entity.Id, tmdbId, containerKey);
            }
        }

        if (updated.Count > 0)
        {
            try
            {
                await repository.BulkUpdateEntitiesAsync(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Type} {Container}: saving {Count} updated entities failed",
                    typeName, containerKey, updated.Count);
                return;
            }
        }

        _logger.LogInformation(
            "{Type} {Container}: filled {Updated} of {Total} (no TMDB image: {NoImage}, no TmdbId: {NoTmdbId}, failed: {Failed})",
            typeName, containerKey, updated.Count, entityList.Count, noImageOnTmdb, noTmdbId, failed);
    }

    private async Task<string?> CreateImageIntoBlobStorage(string containerName, string size, string? tmdbImageFileName)
    {
        if (string.IsNullOrEmpty(tmdbImageFileName))
            return null;

        await using Stream? imageStream = await _tmDBHandler.GetImageStreamAsync($"{size}{tmdbImageFileName}");

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