using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.RemoveItemFromCartUseCase;

internal class RemoveItemFromCartRequestHandler(ITemporaryStorageService temporaryStorage) : IRequestHandler<RemoveItemFromCartRequest>
{
    public async Task Handle(RemoveItemFromCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        cart.Remove(request.itemId);

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
