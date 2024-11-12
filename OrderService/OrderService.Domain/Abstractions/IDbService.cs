using OrderService.Domain.Models;

namespace OrderService.Domain.Abstractions;

public interface IDbService
{
	Task CreateOrderAsync(Order order);
	Task<List<Order>> GetOrdersListAsync();
	Task AddProductToOrderAsync(string orderId, Product product);
}
