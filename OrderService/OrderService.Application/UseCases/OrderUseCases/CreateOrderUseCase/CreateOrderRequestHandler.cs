using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

internal class CreateOrderRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService, 
    IOrderRepository orderRepository, IMapper mapper)
    : IRequestHandler<CreateOrderRequest>
{
    public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();
        var cartDto = mapper.Map<CartDTO>(cart);

        var products = cartDto.Products;

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
            CreatedAt = DateTime.UtcNow,
            Products = orderProducts.ToList(),
            TotalPrice = cartDto.TotalCost,
            UserId = request.userId
        };

        await orderRepository.CreateAsync(order, cancellationToken);

        cart.Clear();
        temporaryStorage.SaveCart(cart);
    }
}
