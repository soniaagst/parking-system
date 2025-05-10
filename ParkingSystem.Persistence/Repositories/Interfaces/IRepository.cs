using System.Linq.Expressions;

namespace ParkingSystem.Persistence.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<List<T>> GetAllAsync();
    Task<T?> FindByIdAsync(Guid id);
    Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression);
    Task<T?> FindFirstAsync(Expression<Func<T, bool>> expression);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(List<T> entity);
}
