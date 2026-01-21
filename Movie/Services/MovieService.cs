using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Dtos.Movies.Request;
using Movie.Dtos.Movies.Response;
using Movie.Entities;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

namespace Movie.Services
{
  public class MovieService : IMovieService
  {
        private readonly IMovieRepositiory _movieRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepositiory movieRepository, IDirectorRepository directorRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _directorRepository = directorRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }
        public async Task<MovieResponseDto> CreateMovie(CreateMovieRequest dto)
        {
            var directorName = dto.DirectorName.Trim();
            var genreName = dto.GenreName.Trim();

            if(string.IsNullOrWhiteSpace(directorName) || string.IsNullOrWhiteSpace(genreName))
            {
                throw new ArgumentException("Director name and genre name cannot be empty.");
            }

            var movie = _mapper.Map<MovieEntity>(dto);

            var director = await _directorRepository.GetDirectorByNameAsync(directorName);
            var genre = await _genreRepository.GetGenreByNameAsync(genreName);

            if(director == null)
            {
                director = new Director{ Name = directorName };

                await _directorRepository.AddAsync(director);
            }

            if(genre == null)
            {
                genre = new Genre{ Name = genreName };

                await _genreRepository.AddGenreAsync(genre);
            }

            movie.Director = director;
            movie.Genre = genre;

            await _movieRepository.AddAsync(movie);
            return _mapper.Map<MovieResponseDto>(movie);
        }

        public async Task DeleteMovie(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id)
                ?? throw new KeyNotFoundException($"Movie with id {id} not found.");

            await _movieRepository.Delete(movie);
        }

        public async Task<List<MovieResponseDto>> GetAllMovies()
        {
            var movies = await _movieRepository.GetMoviesAsync();
            return _mapper.Map<List<MovieResponseDto>>(movies);
        }

        public async Task<MovieResponseDto> GetMovieById(long id)
        {
            var movie = await _movieRepository.GetMovieAsync(id);
            return _mapper.Map<MovieResponseDto>(movie);
        }

        public async Task<MovieResponseDto> UpdateMovie(long id, UpdateMovieRequestDto dto)
        {
            var movie = await _movieRepository.GetMovieAsync(id)
                ?? throw new KeyNotFoundException($"Movie with id {id} not found.");
            
            _mapper.Map(dto, movie);

            if(dto.DirectorName != movie.Director?.Name)
            {
                var director = await _directorRepository.GetDirectorByNameAsync(dto.DirectorName.Trim());
                if(director == null)
                {
                    director = new Director{ Name = dto.DirectorName.Trim() };
                    await _directorRepository.AddAsync(director);
                }
                movie.Director = director;
            }

            if(dto.GenreName != movie.Genre?.Name)
            {
                var genre = await _genreRepository.GetGenreByNameAsync(dto.GenreName.Trim());
                if(genre == null)
                {
                    genre = new Genre{ Name = dto.GenreName.Trim() };
                    await _genreRepository.AddGenreAsync(genre);
                }
                movie.Genre = genre;
            }

            movie.LastUpdatedAt = DateTime.UtcNow;

            await _movieRepository.UpdateAsync(movie);

            return _mapper.Map<MovieResponseDto>(movie);

        }
  }
}
