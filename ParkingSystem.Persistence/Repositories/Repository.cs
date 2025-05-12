using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Persistence.Data;
using ParkingSystem.Persistence.Repositories.Interfaces;

namespace ParkingSystem.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ParkingDbContext _dbContext;
    private DbSet<T> _dbSet;

    public Repository(ParkingDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<T>> GetAllAsync(string? propertiesToInclude = null)
    {
        IQueryable<T> entities = _dbSet;
        if (propertiesToInclude is not null)
            entities = IncludeProperties(entities, propertiesToInclude);
        return await entities.ToListAsync();
    }

    public async Task<T?> FindByIdAsync(Guid id, string? propertiesToInclude = null)
    {
        IQueryable<T> entities = _dbSet;
        if (propertiesToInclude is not null)
            entities = IncludeProperties(entities, propertiesToInclude);
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression, string? propertiesToInclude = null)
    {
        IQueryable<T> entities = _dbSet;
        if (propertiesToInclude is not null)
            entities = IncludeProperties(entities, propertiesToInclude);
        return await entities.Where(expression).ToListAsync();
    }

    public async Task<T?> FindFirstAsync(Expression<Func<T, bool>> expression, string? propertiesToInclude = null)
    {
        IQueryable<T> entities = _dbSet;
        if (propertiesToInclude is not null)
            entities = IncludeProperties(entities, propertiesToInclude);
        return await entities.FirstOrDefaultAsync(expression);
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveRangeAsync(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
        await _dbContext.SaveChangesAsync();
    }

    private static IQueryable<T> IncludeProperties(IQueryable<T> entities, string properties)
    {
        string[] propertiesToInclude = properties.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var property in propertiesToInclude)
        {
            entities = entities.Include(property);
        }
        return entities;
    }

}
