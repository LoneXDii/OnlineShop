using ProductsService.Domain.Common.Models;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Database;

public interface IQueryRepository<T> where T : IEntity
{
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object>>[] includedProperties);
    Task<IEnumerable<T>> ListAllAsync();
    Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>> filter,
                                   params Expression<Func<T, object>>[] includedProperties);
    Task<PaginatedListModel<T>> ListWithPaginationAsync(int pageNo, int pageSize,
                                                        Expression<Func<T, bool>>? filter = null,
                                                        params Expression<Func<T, object>>[] includedProperties);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter);
}
