using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities.Abstractions;
using ProductsService.Infrastructure.Data;

namespace ProductsService.Infrastructure.Repositories;

internal class CommandRepository<T> : ICommandRepository<T> where T : class, IEntity
{
    private readonly CommandDbContext _dbContext;
    private readonly DbSet<T> _entities;

    public CommandRepository(CommandDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _entities.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        _entities.Remove(entity);

        return;
    }
}
