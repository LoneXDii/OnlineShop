using MediatR;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.ClearCartUseCase;

internal class ClearCartRequestHandler(ITemporaryStorageService temporaryStorage) : IRequestHandler<ClearCartRequest>
{
    public async Task Handle(ClearCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        cart.Clear();

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
