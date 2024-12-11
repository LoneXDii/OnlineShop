using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Specifications;
using System.Linq.Expressions;

namespace ProductsService.Infrastructure.Repositories;

internal class QueryRepository<T> : IQueryRepository<T> where T : class, IEntity
{
    private readonly QueryDbContext _dbContext;
    private readonly DbSet<T> _entities;

    public QueryRepository(QueryDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _entities.FindAsync([id], cancellationToken);

        return entity;
    }

    public async Task<T?> GetByIdAsync(int id, ISpecification<T>? specification, CancellationToken cancellationToken = default)
    {
        IQueryable<T>? query = _entities.AsQueryable();
        query = specification?.GetQuery(query);

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _entities.AsQueryable().ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        query = specification.GetQuery(query);

        var entities = await query.ToListAsync();

        return entities;
    }

    public async Task<List<T>> ListWithPaginationAsync(int pageNo, int pageSize, 
        ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        query = specification.GetQuery(query);

        var entities = await query.OrderBy(e => e.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return entities;
    }

    public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        query = specification.GetQuery(query);

        var entity = await query.FirstOrDefaultAsync(cancellationToken);

        return entity;
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        query = specification.GetQuery(query);

        var count = await query.CountAsync(cancellationToken); 
        
        return count;
    }
}
