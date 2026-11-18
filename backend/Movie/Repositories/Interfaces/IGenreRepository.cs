using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllAsync();
        Task<Genre?> GetGenreByIdAsync(long id);
        Task AddGenreAsync(Genre genre);
        Task UpdateAsync(Genre genre);
        Task Delete(Genre genre);
        Task<Genre?> GetGenreByNameAsync(string name);
    }
}