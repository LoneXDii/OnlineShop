using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using ProductsService.Infrastructure.Data;
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

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();
        query = query.Where(specification.ToExpression());

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<T>> ListWithPaginationAsync(int pageNo, int pageSize, 
        ISpecification<T> specification, CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();
        query = query.Where(specification.ToExpression());

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return await query.OrderBy(e => e.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default, 
        params Expression<Func<T, object>>[] includedProperties)
    {
        var query = _entities.AsQueryable();
        query = query.Where(specification.ToExpression());

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return await query.FirstOrDefaultAsync(cancellationToken); ;
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        var query = _entities.AsQueryable();
        query = query.Where(specification.ToExpression());
        
        return await query.CountAsync(cancellationToken);
    }
}
