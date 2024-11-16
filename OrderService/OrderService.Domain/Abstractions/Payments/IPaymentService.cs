using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Payments;

public interface IPaymentService
{
    Task<string> PayAsync(OrderEntity order, string customerId);
    Task<string> CreateCustomerAsync(string email, string name);
    Task<string> CreateProductAsync(ProductEntity product);
    string? GetSuccessPaymentOrderId(string eventJson, string signature);
}
