using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

internal class RemoveItemFromCartRequestHandler(ITemporaryStorageService temporaryStorage) : IRequestHandler<RemoveItemFromCartRequest>
{
    public Task Handle(RemoveItemFromCartRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();
        cart.Remove(request.itemId);
        temporaryStorage.SaveCart(cart);

        return Task.CompletedTask;
    }
}
