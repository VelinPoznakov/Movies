using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Movie.Dtos.Director.Request;
using Movie.Dtos.Director.Response;
using Movie.Entities;
using Movie.Repositories.Interfaces;
using Movie.Services.Interfases;

namespace Movie.Services
{
  public class DirectorService : IDirectorService
  {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;

        public DirectorService(IDirectorRepository directorRepository, IMapper mapper)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
        }
        public async Task<DirectorReasponse> CreateDirectorAsync(CreateDirectorRequestDto dto)
        {
            var director = _mapper.Map<Director>(dto);
            await _directorRepository.AddAsync(director);

            return _mapper.Map<DirectorReasponse>(director);
        }

        public async Task DeleteDirectorAsync(long id)
        {
            var director = await _directorRepository.GetDirectorByIdAsync(id)
                ?? throw new Exception("Director not found");

            await _directorRepository.Delete(director);
        }

        public async Task<List<DirectorReasponse>> GetAllDirectorsAsync()
        {
            var director = await _directorRepository.GetAllAsync();

            return _mapper.Map<List<DirectorReasponse>>(director);
        }

        public async Task<DirectorReasponse> GetDirectorByIdAsync(long id)
        {
            var director = await _directorRepository.GetDirectorByIdAsync(id)
                ?? throw new Exception("Director not found");

            return _mapper.Map<DirectorReasponse>(director);
        }

        public async Task<DirectorReasponse> UpdateDirectorAsync(long id, UpdateDirectorRequestDto dto)
        {
            var director = await _directorRepository.GetDirectorByIdAsync(id)
                ?? throw new Exception("Director not found");
            
            _mapper.Map(dto, director);

            await _directorRepository.UpdateAsync(director);

            return _mapper.Map<DirectorReasponse>(director);
        }
  }
}