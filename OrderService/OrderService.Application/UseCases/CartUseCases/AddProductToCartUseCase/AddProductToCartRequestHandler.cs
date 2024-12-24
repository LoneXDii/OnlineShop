using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;

internal class AddProductToCartRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService,
    ILogger<AddProductToCartRequestHandler> logger)
    : IRequestHandler<AddProductToCartRequest>
{
    public async Task Handle(AddProductToCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        var product = await productService.GetByIdIfSufficientQuantityAsync(request.product.Id, 
            GetProductQuaintity(request, cart), cancellationToken);

        if (product is null)
        {
            logger.LogError($"Product with id: {request.product.Id} not exists or it's quantity is to low for {GetProductQuaintity(request, cart)}");

            throw new NotFoundException("Cannot add to cart, this product not exist or its quantity to low");
        }

        if (cart.ContainsKey(product.Id))
        {
            cart[product.Id] = product;
        }
        else
        {
            cart.Add(product.Id, product);
        }

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }

    private int GetProductQuaintity(AddProductToCartRequest request, Dictionary<int, ProductEntity> cart)
    { 
        var cartProductQuantity = 0;

        if (cart.ContainsKey(request.product.Id))
        {
            cartProductQuantity = cart[request.product.Id].Quantity;
        }

        return request.product.Quantity + cartProductQuantity;
    }
}
