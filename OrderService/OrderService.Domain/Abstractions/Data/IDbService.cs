using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Domain.Abstractions.Data;

public interface IDbService
{
    Task CreateOrderAsync(OrderEntity order);
    Task<PaginatedListModel<OrderEntity>> ListOrdersWithPaginationAsync(int pageNo = 1, int pageSize = 10, params Expression<Func<OrderEntity, bool>>[] filters);
    Task<OrderEntity?> GetOrderByIdAsync(string id);
    Task UpdateOrderAsync(OrderEntity order);
}
