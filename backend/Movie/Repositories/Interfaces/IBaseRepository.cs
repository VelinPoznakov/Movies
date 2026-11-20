using System.Linq.Expressions;

namespace Movie.Repositories.Interfaces;

public interface IBaseRepository<T>
{
    Task<bool> CreateEntityAsync(T entity);

    Task<bool> BulkCreateEntitiesAsync(ICollection<T> entities);

    Task<bool> DeleteEntityAsync(T entity);

    Task<bool> BulkDeleteEntitiesAsync(ICollection<T> entities);

    Task<T?> GetEntityByIdAsync(
        long id,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, T>>? projection = null,
        bool searchByTmdbId = false,
        bool asNoTracking = true,
        bool asSplitQuery = true);

    Task<bool> UpdateEntityAsync(T entity);

    Task<bool> BulkUpdateEntitiesAsync(ICollection<T> entities);

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Dictionary<Expression<Func<T, object>>, bool>? orders = null,
        Expression<Func<T, T>>? projections = null,
        int skip = 0,
        int take = 0,
        bool asNoTracking = true,
        bool asSplitQuery = true);

    Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

    Task<bool> DoesEntityExistByTmdbIdAsync(long tmdbId);

    void ClearTracking();
}