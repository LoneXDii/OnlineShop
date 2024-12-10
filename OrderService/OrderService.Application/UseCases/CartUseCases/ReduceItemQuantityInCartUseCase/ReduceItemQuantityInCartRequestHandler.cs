using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

internal class ReduceItemQuantityInCartRequestHandler(ITemporaryStorageService temporaryStorage)
    : IRequestHandler<ReduceItemsInCartRequest>
{
    public Task Handle(ReduceItemsInCartRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();

        if (cart.ContainsKey(request.ProductId))
        {
            cart[request.ProductId].Quantity -= request.Quantity;

            if (cart[request.ProductId].Quantity < 0)
            {
                cart.Remove(request.ProductId);
            }
        }

        temporaryStorage.SaveCart(cart);

        return Task.CompletedTask;
    }
}
