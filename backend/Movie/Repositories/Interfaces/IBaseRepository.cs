using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Movie.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<bool> CreateEntityAsync(T entity);

    Task<bool> BulkCreateEntitiesAsync(ICollection<T> entities);

    Task<bool> DeleteEntityAsync(T entity);

    Task<bool> BulkDeleteEntitiesAsync(ICollection<T> entities);

    Task<TResult?> GetEntityByIdAsync<TResult>(
        long id,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, TResult>>? projection = null,
        bool searchByTmdbId = false,
        bool asNoTracking = true,
        bool asSplitQuery = true);

    Task<bool> UpdateEntityAsync(T entity);

    Task<bool> BulkUpdateEntitiesAsync(ICollection<T> entities);

    Task<List<TResult>> GetAllAsync<TResult>(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Dictionary<Expression<Func<T, object>>, bool>? orders = null,
        Expression<Func<T, TResult>>? projection = null,
        int skip = 0,
        int take = 0,
        bool asNoTracking = true,
        bool asSplitQuery = true);

    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    Task<bool> DoesEntityExistByTmdbIdAsync(long tmdbId);
    
    Task<bool> UpdateEntityPropertyAsync(
        Expression<Func<T, bool>> filter,
        Action<UpdateSettersBuilder<T>> setters);

    void ClearTracking();
}