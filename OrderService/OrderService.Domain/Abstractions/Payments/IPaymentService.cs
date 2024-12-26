﻿using OrderService.Domain.Entities;

namespace OrderService.Domain.Abstractions.Payments;

public interface IPaymentService
{
    Task<string> PayAsync(OrderEntity order, string customerId);
    Task<string> CreateCustomerAsync(string email, string name);
    Task<string> CreateProductAsync(string name, double price);
    string? GetSuccessPaymentOrderId(string eventJson, string signature);
}
