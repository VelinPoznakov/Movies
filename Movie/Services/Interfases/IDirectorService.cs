using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Dtos.Director.Request;
using Movie.Dtos.Director.Response;

namespace Movie.Services.Interfases
{
    public interface IDirectorService
    {
        Task<DirectorReasponse> GetDirectorByIdAsync(long id);
        Task<List<DirectorReasponse>> GetAllDirectorsAsync();
        Task<DirectorReasponse> CreateDirectorAsync(CreateDirectorRequestDto dto);
        Task<DirectorReasponse> UpdateDirectorAsync(long id, UpdateDirectorRequestDto dto);
        Task DeleteDirectorAsync(long id);
    }
}