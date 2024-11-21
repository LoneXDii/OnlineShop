using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Common.Models;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Database;

public interface IQueryRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includedProperties);
    Task<IEnumerable<T>> ListAllAsync();
    Task<IEnumerable<T>> ListAsync(ISpecification<T> specification);
    Task<PaginatedListModel<T>> ListWithPaginationAsync(int pageNo, int pageSize, ISpecification<T> specification);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
}
