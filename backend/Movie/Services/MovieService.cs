using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movie.Dtos.Comments;
using Movie.Dtos.Director;
using Movie.Dtos.Genre;
using Movie.Dtos.Movies;
using Movie.Dtos.ProducerComapny;
using Movie.Entities;
using Movie.Exceptions;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

using static Movie.Common.ApplicationConstans;

namespace Movie.Services
{
  public class MovieService : IMovieService
  {
      private readonly IBaseRepository<MovieEntity> _movieRepository;
      
      public MovieService(IBaseRepository<MovieEntity> movieRepository)
      {
          _movieRepository = movieRepository;
      }

      public async Task<MovieDetailsResponseDto> GetMovieDetailsAsync(long id)
      {
          MovieDetailsResponseDto? movieDto = await _movieRepository
              .GetEntityByIdAsync<MovieDetailsResponseDto>(
                  id,
                  include: m => m
                      .Include(r => r.Ratings)
                      .Include(g => g.MoviesGenres)
                      .Include(cp => cp.MoviesProducerCompanies)
                      .Include(d => d.MoviesDirectors)
                      .Include(c => c.Comments),
                  projection: m => new MovieDetailsResponseDto()
                  {
                      Id = m.Id,
                      Title = m.Title,
                      Description = m.Description,
                      TimeLong = m.TimeLong,
                      Budget = m.Budget,
                      MovieStatus = m.Status.ToString(),
                      BackdropUrl = m.BackDroopPoster,
                      ReleaseDate = m.ReleaseDate.ToString("dd-MM-yyyy"),
                      IsAdult = m.IsAdult,
                      Rating = m.Ratings.Any() ? m.Ratings.Average(r => r.Value) : 0,
                      Genres = m.MoviesGenres.Select(g => new GenreDto()
                          {
                              Id = g.GenreId,
                              Name = g.Genre.Name
                          })
                          .ToList(),
                      ProducerCompanies = m.MoviesProducerCompanies.Select(pc => new ProducerCompanyDetails()
                          {
                              Id = pc.ProducerCompanyId,
                              Name = pc.ProducerCompany.Name,
                              LogoUrl = pc.ProducerCompany.LogoPath
                          })
                          .ToList(),
                      Directors = m.MoviesDirectors.Select(d => new DirectorDto()
                          {
                              Id = d.DirectorId,
                              Name = d.Director.Name,
                              ProfilePath = d.Director.ProfilePath
                          })
                          .ToList(),
                      Comments = m.Comments.Select(c => new CommentDto()
                          {
                              Id = c.Id,
                              Content = c.Content,
                              Username = c.User.UserName!,
                              CreatedAt = c.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss")
                          })
                          .ToList()
                  },
                  asNoTracking: false
              );

          if (movieDto == null)
              throw new EntityNotFoundException("Entity was not found!");

          MovieEntity? movie = await _movieRepository
              .GetEntityByIdAsync(
                  id: id,
                  projection: m => new MovieEntity() { Id = m.Id, Views = m.Views }
              );

          if (movie != null)
          {
              await _movieRepository
                  .UpdateEntityPropertyAsync(
                      filter: m => m.Id == id,
                      m => m.SetProperty(v => v.Views, v => v.Views + 1));
          }
          
          return movieDto;
      }

      public async Task<MovieCollectionResponseDto> GetMovieCollectionsAsync(int page)
      {
          List<MovieFromCollectionDto> moviesDto = await _movieRepository
              .GetAllAsync<MovieFromCollectionDto>(
                  orders: new Dictionary<Expression<Func<MovieEntity, object>>, bool>()
                  {
                      [m => m.Ratings.Average(r => (double?)r.Value) ?? 0] = false,
                      [v => v.Views] = false,
                      [t => t.Title] = true,
                      [c => c.CreatedAt] = true,
                      [i => i.Id] = false
                  },
                  include: m => m
                      .Include(r => r.Ratings),
                  projection: m => new MovieFromCollectionDto()
                  {
                      Id = m.Id,
                      Title = m.Title,
                      TimeLong = m.TimeLong,
                      MovieStatus = m.Status.ToString(),
                      PosterUrl = m.PosterBlobName,
                      Rating = m.Ratings.Any() ? m.Ratings.Average(r => r.Value) : 0,
                      Views =  m.Views,
                  },
                  skip: (page - 1) * PageMaxPageSize,
                  take: PageMaxPageSize);

          int moviesCount = await _movieRepository
              .CountAsync();

          return new MovieCollectionResponseDto()
          {
              Movies = moviesDto,
              MoviesPerPage = PageMaxPageSize,
              TotalPages = (int)Math.Ceiling((double)moviesCount / PageMaxPageSize)
          };
      }
  }
}
