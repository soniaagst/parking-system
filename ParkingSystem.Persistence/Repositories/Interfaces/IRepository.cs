using System.Linq.Expressions;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<List<T>> GetAllAsync(string? propertiesToInclude = null);
    Task<T?> FindByIdAsync(Guid id, string? propertiesToInclude = null);
    Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression, string? propertiesToInclude = null);
    Task<T?> FindFirstAsync(Expression<Func<T, bool>> expression, string? propertiesToInclude = null);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(List<T> entity);
}
