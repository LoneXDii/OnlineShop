using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

internal class SetItemQuantityInCartRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService)
    : IRequestHandler<SetItemQuantityInCartRequest>
{
    public async Task Handle(SetItemQuantityInCartRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdIfSufficientQuantityAsync(request.ProductId, request.Quantity, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException("Cannot add to cart, this product not exist or its quantity to low");
        }

        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        cart[product.Id] = product;

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
