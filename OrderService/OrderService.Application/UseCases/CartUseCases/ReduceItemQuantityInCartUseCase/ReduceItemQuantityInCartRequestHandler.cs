﻿using MediatR;
using OrderService.Domain.Abstractions.Cart;

namespace OrderService.Application.UseCases.CartUseCases.ReduceItemQuantityInCartUseCase;

internal class ReduceItemQuantityInCartRequestHandler(Cart cart)
	: IRequestHandler<ReduceItemsInCartRequest>
{
	public async Task Handle(ReduceItemsInCartRequest request, CancellationToken cancellationToken)
	{
		cart.ReduceInCart(request.product.Id, request.product.Quantity);
	}
}
