using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Common.Models;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Database;

public interface IQueryRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includedProperties);
    Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<PaginatedListModel<T>> ListWithPaginationAsync(int pageNo, int pageSize, ISpecification<T> specification, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
}
