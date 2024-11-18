using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Domain.Abstractions.Data;

public interface IOrderRepository
{
    Task CreateAsync(OrderEntity order);
    Task<PaginatedListModel<OrderEntity>> ListWithPaginationAsync(int pageNo = 1, int pageSize = 10, params Expression<Func<OrderEntity, bool>>[] filters);
    Task<OrderEntity?> GetByIdAsync(string id);
    Task UpdateAsync(OrderEntity order);
}
