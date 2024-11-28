using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

internal class ReduceItemQuantityInCartRequestHandler(ITemporaryStorageService temporaryStorage)
    : IRequestHandler<ReduceItemsInCartRequest>
{
    public Task Handle(ReduceItemsInCartRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();

        if (cart.ContainsKey(request.product.Id))
        {
            cart[request.product.Id].Quantity -= request.product.Quantity;

            if (cart[request.product.Id].Quantity < 0)
            {
                cart.Remove(request.product.Id);
            }
        }

        temporaryStorage.SaveCart(cart);

        return Task.CompletedTask;
    }
}
