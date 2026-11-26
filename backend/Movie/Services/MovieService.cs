using Microsoft.EntityFrameworkCore;
using Movie.Common;
using Movie.Dtos.Comments;
using Movie.Dtos.Director;
using Movie.Dtos.Genre;
using Movie.Dtos.Movies;
using Movie.Dtos.ProducerComapny;
using Movie.Entities;
using Movie.Exceptions;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

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
                  }
              );

          if (movieDto == null)
              throw new EntityNotFoundException("Entity was not found!");
          
          return movieDto;
      }
  }
}
