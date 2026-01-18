using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Entities;
using Movie.Repositories.Interfaces;

namespace Movie.Repositories
{
  public class DirectorRepository : IDirectorRepository
  {
        private readonly MovieDbContext _context;

        public DirectorRepository(MovieDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Director director)
        {
            await _context.Directors.AddAsync(director);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Director director)
        {
            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Director>> GetAllAsync()
        {
            return await _context.Directors.ToListAsync();
        }

        public async Task<Director?> GetDirectorByIdAsync(long id)
        {
            return await _context.Directors.FindAsync(id);
        }

        public async Task UpdateAsync(Director director)
        {
            _context.Directors.Update(director);
            await _context.SaveChangesAsync();
        }

        public async Task<Director?> GetDirectorByNameAsync(string name)
        {
            return await _context.Directors.FirstOrDefaultAsync(d => d.Name.ToLower() == name.ToLower());
        }
  }
}