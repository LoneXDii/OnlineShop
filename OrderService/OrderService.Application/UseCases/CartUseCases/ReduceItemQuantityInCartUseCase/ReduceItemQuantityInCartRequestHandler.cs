using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

internal class ReduceItemQuantityInCartRequestHandler(ITemporaryStorageService temporaryStorage)
    : IRequestHandler<ReduceItemsInCartRequest>
{
    public async Task Handle(ReduceItemsInCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        if (cart.ContainsKey(request.ProductId))
        {
            cart[request.ProductId].Quantity -= request.Quantity;

            if (cart[request.ProductId].Quantity < 0)
            {
                cart.Remove(request.ProductId);
            }
        }

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
