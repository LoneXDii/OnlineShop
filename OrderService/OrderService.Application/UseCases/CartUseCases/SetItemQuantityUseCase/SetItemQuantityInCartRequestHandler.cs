﻿using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Data;

namespace OrderService.Application.UseCases.CartUseCases.SetItemQuantityInCartUseCase;

internal class SetItemQuantityInCartRequestHandler(ITemporaryStorageService temporaryStorage, IProductService productService)
    : IRequestHandler<SetItemQuantityInCartRequest>
{
    public async Task Handle(SetItemQuantityInCartRequest request, CancellationToken cancellationToken)
    {
        var product = await productService.GetByIdIfSufficientQuantityAsync(request.product.Id, request.product.Quantity);

        if (product is null)
        {
            throw new NotFoundException("Cannot add to cart, this product not exist or its quantity to low");
        }

        var cart = await temporaryStorage.GetCartAsync(cancellationToken);

        cart.Remove(product.Id);
        cart.Add(product.Id, product);

        await temporaryStorage.SaveCartAsync(cart, cancellationToken);
    }
}
