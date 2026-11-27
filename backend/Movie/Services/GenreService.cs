using Movie.Dtos.Genre;
using Movie.Entities;
using Movie.Exceptions;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

using static Movie.Common.ApplicationConstans;

namespace Movie.Services;

public class GenreService: IGenreService
{
    private readonly IBaseRepository<Genre> _genreRepository;
    
    public GenreService(IBaseRepository<Genre> genreRepository)
    {
        _genreRepository = genreRepository;
    }
    
    public async Task<GenreCollectionDto> GetAllGenresAsync(int page)
    {
        List<GenreDto> genresDto = await _genreRepository
            .GetAllAsync<GenreDto>(
                projection: g => new GenreDto()
                {
                    Id = g.Id,
                    Name = g.Name
                },
                skip: (page - 1) * PageMaxPageSize,
                take: PageMaxPageSize
            );

        int totalCount = await _genreRepository
            .CountAsync();

        return new GenreCollectionDto()
        {
            Genres = genresDto,
            Page = page,
            TotalPages = (int)Math.Ceiling((double)totalCount / PageMaxPageSize)
        };
    }

    public async Task<GenreWithMoviesDto> GetGenreDetailsWithMoviesAsync(long id, int page)
    {
        GenreWithMoviesDto? genreWithMoviesDto = await _genreRepository
            .GetEntityByIdAsync<GenreWithMoviesDto>(
                id,
                projection: g => new GenreWithMoviesDto()
                {
                    Id = g.Id,
                    Name = g.Name,
                    Page = page,
                    TotalPages = (g.MoviesGenres.Count() + PageMaxPageSize - 1) / PageMaxPageSize,
                    Movies = g.MoviesGenres
                        .OrderByDescending(mg => mg.Movie.Ratings.Average(r => (double?)r.Value) ?? 0)
                        .ThenByDescending(mg => mg.Movie.Views)
                        .ThenBy(mg => mg.Movie.Title)
                        .ThenBy(mg => mg.Movie.CreatedAt)
                        .ThenByDescending(mg => mg.MovieId)
                        .Select(mg => new Dtos.Movies.MovieFromCollectionDto()
                        {
                            Id = mg.MovieId,
                            Title = mg.Movie.Title,
                            TimeLong = mg.Movie.TimeLong,
                            MovieStatus = mg.Movie.Status.ToString(),
                            PosterUrl = mg.Movie.PosterBlobName,
                            Rating = mg.Movie.Ratings.Any() ? mg.Movie.Ratings.Average(r => r.Value) : 0,
                            Views = mg.Movie.Views,
                        }
                    )
                    .Skip((page - 1) * PageMaxPageSize)
                    .Take(PageMaxPageSize)
                    .ToList()
                }
            );
        
        if(genreWithMoviesDto == null)
            throw new EntityNotFoundException("Entity was not found!");

        return genreWithMoviesDto;
    }
}