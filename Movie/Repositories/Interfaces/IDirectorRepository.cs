using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.Entities;

namespace Movie.Repositories.Interfaces
{
    public interface IDirectorRepository
    {
        Task <List<Director>> GetAllAsync();
        Task <Director?> GetDirectorByIdAsync(long id);
        Task AddAsync(Director director);
        Task Delete(Director director);
        Task UpdateAsync(Director director);
        Task<Director?> GetDirectorByNameAsync(string name);
    }
}