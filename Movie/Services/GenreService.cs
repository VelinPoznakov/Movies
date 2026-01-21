

using AutoMapper;
using Movie.Dtos.Genre.Request;
using Movie.Dtos.Genre.Response;
using Movie.Entities;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfaces;

namespace Movie.Services
{
    public class GenreService: IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<GenreResponse> CreateGenreAsync(CreateGenreRequestDto dto)
        {
            var genre = _mapper.Map<Genre>(dto);

            await _genreRepository.AddGenreAsync(genre);

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task DeleteGenreAsync(long id)
        {
            var genre = await _genreRepository.GetGenreByIdAsync(id)
                ?? throw new Exception("Genre not found");

            await _genreRepository.Delete(genre);
        }

        public async Task<List<GenreResponse>> GetAllGenreAsync()
        {
            var genres = await _genreRepository.GetAllAsync();

            return _mapper.Map<List<GenreResponse>>(genres);
        }

        public async Task<GenreResponse> GetGenreByIdAsync(long id)
        {
            var genre = await _genreRepository.GetGenreByIdAsync(id);

            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> UpdateGenreAsync(long id, UpdateGenreRequestDto dto)
        {
            var genre = await _genreRepository.GetGenreByIdAsync(id)
                ?? throw new Exception("Genre not found");
            
            _mapper.Map(dto, genre);

            await _genreRepository.UpdateAsync(genre);

            return _mapper.Map<GenreResponse>(genre);
        }
  }
}