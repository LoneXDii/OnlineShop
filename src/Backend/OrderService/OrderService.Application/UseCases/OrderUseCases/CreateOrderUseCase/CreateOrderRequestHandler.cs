using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

internal class CreateOrderRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService, 
    IOrderRepository orderRepository, IMapper mapper, IProducerService producerService,
    ILogger<CreateOrderRequestHandler> logger)
    : IRequestHandler<CreateOrderRequest>
{
    public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.userId} trying to create an order");

        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        var cartDto = mapper.Map<CartDTO>(cart);

        var products = cartDto.Products;

        if (!products.Any()) 
        {
            logger.LogError($"Cart of user with id: {request.userId} is empty");

            throw new BadRequestException("Your cart is empty");
        }

        var orderProducts = await productService.TakeProductsIfSufficientQuantityAsync(products, cancellationToken);

        if(orderProducts is null)
        {
            throw new BadRequestException("Not enought products in stock");
        }

        var order = new OrderEntity { 
            CreatedAt = DateTime.UtcNow,
            Products = orderProducts.ToList(),
            TotalPrice = cartDto.TotalCost,
            UserId = request.userId
        };

        await orderRepository.CreateAsync(order, cancellationToken);

        cart.Clear();

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);

        await producerService.ProduceOrderActionsAsync(order, cancellationToken);

        logger.LogInformation($"User with id: {request.userId} successfully created an order");
    }
}
