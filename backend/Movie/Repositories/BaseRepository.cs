using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Movie.Data;
using Movie.Repositories.Interfaces;

namespace Movie.Repositories;

public class BaseRepository<T>: IBaseRepository<T> where T : class
{
    protected readonly MovieDbContext _context;
    
    public BaseRepository(MovieDbContext context)
    {
        _context = context;
    }

    protected async Task<int> SaveChangesAsync() 
        => await _context.SaveChangesAsync();

    public async Task<bool> CreateEntityAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);

        int result = await SaveChangesAsync();

        return result == 1;
    }
    
    public async Task<bool> BulkCreateEntitiesAsync(ICollection<T> entities)
    {
        await _context.Set<T>().AddRangeAsync(entities);

        int result = await SaveChangesAsync();

        return result == entities.Count;
    }
    
    public async Task<bool> DeleteEntityAsync(T entity)
    {
        _context.Set<T>().Remove(entity);

        int result = await SaveChangesAsync();

        return result == 1;
    }
    
    public async Task<bool> BulkDeleteEntitiesAsync(ICollection<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);

        int result = await SaveChangesAsync();

        return result == entities.Count;
    }
    
    public async Task<T?> GetEntityByIdAsync(
        int id,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, T>>? projection = null,
        bool searchByTmdbId = false,
        bool asNoTracking = true,
        bool asSplitQuery = true)
    {
        IQueryable<T> query = _context.Set<T>();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (include != null)
        {
            query = include(query);

            if (asSplitQuery)
                query = query.AsSplitQuery();
        }

        string column = searchByTmdbId ? "TmdbId" : "Id";

        query = query.Where(e => EF.Property<int>(e, column) == id);

        if (projection != null)
            query = query.Select(projection);

        return await query.FirstOrDefaultAsync();
    }
    
    public async Task<bool> UpdateEntityAsync(T entity)
    {
        _context.Set<T>().Update(entity);

        int result = await SaveChangesAsync();

        return result == 1;
    }
    
    public async Task<bool> BulkUpdateEntitiesAsync(ICollection<T> entities)
    {
        _context.Set<T>().UpdateRange(entities);

        int result = await SaveChangesAsync();

        return result == entities.Count;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Dictionary<Expression<Func<T, object>>, bool>? orders = null,
        Expression<Func<T, T>>? projections = null,
        int skip = 0,
        int take = 0,
        bool asNoTracking = true,
        bool asSplitQuery = true)
    {
        IQueryable<T> query = _context.Set<T>();
        
        if(asNoTracking)
            query = query.AsNoTracking();

        if (include != null)
        {
            query = include(query);
            
            if(asSplitQuery)
                query = query.AsSplitQuery();
        }
        
        if(filter != null)
            query = query.Where(filter);
        
        if(orders != null && orders.Count > 0)
        {
            KeyValuePair<Expression<Func<T, object>>, bool> firstOrder = orders.First();
            
            if(firstOrder.Value)
                query = query.OrderBy(firstOrder.Key);
            else
                query = query.OrderByDescending(firstOrder.Key);
            
            for(int i = 1; i < orders.Count; i++)
            {
                KeyValuePair<Expression<Func<T, object>>, bool> order = orders.ElementAt(i);
                
                if(order.Value)
                    query = ((IOrderedQueryable<T>)query).ThenBy(order.Key);
                else
                    query = ((IOrderedQueryable<T>)query).ThenByDescending(order.Key);
            }
        }
        
        if(projections != null)
            query = query.Select(projections);
        
        if(skip > 0)
            query = query.Skip(skip);
        
        if(take > 0)
            query = query.Take(take);
        
        return await query.ToArrayAsync();
    }

    public async Task<int> CountAsync(
        Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _context.Set<T>();
        
        if(filter != null)
            query = query.Where(filter);
        
        return await query.CountAsync();
    }

    public async Task<bool> DoesEntityExistByTmdbIdAsync(int tmdbId)
    {
        return await _context.Set<T>()
            .AnyAsync(e => EF.Property<int>(e, "TmdbId") == tmdbId);
    }
}