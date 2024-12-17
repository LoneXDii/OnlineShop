using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;

internal class AddProductToCartRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService)
    : IRequestHandler<AddProductToCartRequest>
{
    public async Task Handle(AddProductToCartRequest request, CancellationToken cancellationToken)
    {
        var cartProductQuantity = 0;

        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        if (cart.ContainsKey(request.product.Id))
        {
            cartProductQuantity = cart[request.product.Id].Quantity;
        }

        var product = await productService.GetByIdIfSufficientQuantityAsync(request.product.Id, request.product.Quantity + cartProductQuantity);

        if (product is null)
        {
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
}
