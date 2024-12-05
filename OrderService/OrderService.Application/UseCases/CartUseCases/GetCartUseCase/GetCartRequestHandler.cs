using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

internal class GetCartRequestHandler(ITemporaryStorageService temporaryStorage, IMapper mapper)
    : IRequestHandler<GetCartRequest, CartDTO>
{
    public async Task<CartDTO> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        var cartDto = mapper.Map<CartDTO>(cart);

        return cartDto;
    }
}
