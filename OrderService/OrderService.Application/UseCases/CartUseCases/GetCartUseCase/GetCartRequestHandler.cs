using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

internal class GetCartRequestHandler(ICart cart, IMapper mapper)
    : IRequestHandler<GetCartRequest, CartDTO>
{
    public Task<CartDTO> Handle(GetCartRequest request, CancellationToken cancellationToken)
    {
        var cartDto = mapper.Map<CartDTO>(cart);

        return Task.FromResult(cartDto);
    }
}
