using AutoMapper;
using MediatR;
using OrderService.Application.DTO;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.GetCartUseCase;

internal class GetCartRequestHandler(Cart cart, IMapper mapper)
	: IRequestHandler<GetCartRequest, CartDTO>
{
	public async Task<CartDTO> Handle(GetCartRequest request, CancellationToken cancellationToken)
	{
		var cartDto = mapper.Map<CartDTO>(cart);
		return cartDto;
	}
}
