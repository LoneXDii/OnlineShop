using Microsoft.EntityFrameworkCore;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities.Abstractions;
using SupportService.Infrastructure.Data;
using System.Linq.Expressions;
using System.Linq;
using System.Threading;

namespace SupportService.Infrastructure.Repositories;

internal class Repository<T> : IRepository<T> where T : class, IEntity
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<T> _entities;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _entities.FindAsync([id], cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
    {
        return await _entities.FirstOrDefaultAsync(filter, cancellationToken);
    }
}
