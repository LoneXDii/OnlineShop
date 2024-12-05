using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;

internal class ClearCartRequestHandler(ITemporaryStorageService temporaryStorage) : IRequestHandler<ClearCartRequest>
{
    public Task Handle(ClearCartRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();
        cart.Clear();
        temporaryStorage.SaveCart(cart);

        return Task.CompletedTask;
    }
}
