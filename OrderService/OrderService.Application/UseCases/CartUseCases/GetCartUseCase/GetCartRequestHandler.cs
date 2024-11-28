using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

internal class GetCartRequestHandler(ITemporaryStorageService temporaryStorage, IMapper mapper)
    : IRequestHandler<GetCartRequest, CartDTO>
{
    public Task<CartDTO> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
        var cart = temporaryStorage.GetCart();
        var cartDto = mapper.Map<CartDTO>(cart);

        return Task.FromResult(cartDto);
    }
}
