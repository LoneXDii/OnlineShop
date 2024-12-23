using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.AddProductToCartUseCase;

internal class AddProductToCartRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService)
    : IRequestHandler<AddProductToCartRequest>
{
    public async Task Handle(AddProductToCartRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdIfSufficientQuantityAsync(request.product.Id, request.product.Quantity);

        if (product is null)
        {
            throw new NotFoundException("Cannot add to cart, this product not exist or its quantity to low");
        }

        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        if (cart.ContainsKey(product.Id))
        {
            cart[product.Id].Quantity += product.Quantity;
        }
        else
        {
            cart.Add(product.Id, product);
        }

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
