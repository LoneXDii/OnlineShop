﻿using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

internal class CreateOrderRequestHandler(ICart cart, IProductService productService, IOrderRepository dbService)
    : IRequestHandler<CreateOrderRequest>
{
    public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var products = cart.Items.Values.ToList();

        if (!products.Any()) 
        {
            throw new BadRequestException("Your cart is empty");
        }

        var orderProducts = await productService.TakeProductsIfSufficientQuantityAsync(products);

        if(orderProducts is null)
        {
            throw new BadRequestException("Not enought products in stock");
        }

        var order = new OrderEntity { 
            CreatedAt = DateTime.Now,
            Products = orderProducts.ToList(),
            TotalPrice = cart.TotalCost,
            UserId = request.userId
        };

        await dbService.CreateAsync(order, cancellationToken);

        cart.ClearAll();
    }
}
