using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

internal class SetItemQuantityInCartRequestHandler(Cart cart, IProductService productService)
    : IRequestHandler<SetItemQuantityInCartRequest>
{
    public async Task Handle(SetItemQuantityInCartRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdIfSufficientQuantityAsync(request.product.Id, request.product.Quantity);

        if (product is null)
        {
            throw new NotFoundException("Cannot add to cart, this product not exist or its quantity to low");
        }

        cart.RemoveItems(request.product.Id);
        cart.AddToCart(product);
    }
}
