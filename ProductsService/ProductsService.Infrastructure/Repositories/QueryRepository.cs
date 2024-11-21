using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Common.Models;
using ProductsService.Domain.Entities.Abstractions;
using ProductsService.Infrastructure.Data;
using ProductsService.Infrastructure.Specification;
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

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includedProperties)
    {
        IQueryable<T>? query = _entities.AsQueryable();

        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        var entity = await query.FirstOrDefaultAsync(e => e.Id == id);

        return entity;
    }

    public async Task<IEnumerable<T>> ListAllAsync()
    {
        var entities = await _entities.AsQueryable().ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<T>> ListAsync(ISpecification<T> specification)
    {
        IQueryable<T>? query = _entities.AsQueryable();
        query = SpecificationQueryBuilder.GetQuery(query, specification);

        var entities = await query.ToListAsync();

        return entities;
    }

    public async Task<PaginatedListModel<T>> ListWithPaginationAsync(int pageNo, int pageSize, 
        ISpecification<T> specification)
    {
        var query = _entities.AsQueryable();
        query = SpecificationQueryBuilder.GetQuery(query, specification);

        int count = await query.CountAsync();

        var entities = await query.OrderBy(e => e.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var data = new PaginatedListModel<T>
        {
            Items = entities,
            CurrentPage = pageNo,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        return data;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter)
    {
        var entity = await _entities.FirstOrDefaultAsync(filter);

        return entity;
    }
}
