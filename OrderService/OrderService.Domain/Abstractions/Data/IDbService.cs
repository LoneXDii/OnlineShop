using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;
using System.Linq.Expressions;

namespace OrderService.Domain.Abstractions.Data;

public interface IDbService
{
    Task CreateOrderAsync(Order order);
    Task<PaginatedListModel<Order>> ListOrdersWithPaginationAsync(int pageNo = 1, int pageSize = 10, params Expression<Func<Order, bool>>[] filters);
    Task AddProductToOrderAsync(string orderId, Product product);
    Task DeleteProductFromOrderAsync(string orderId, int productId);
}
