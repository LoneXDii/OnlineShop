using MediatR;
using OrderService.Application.Exceptions;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Application.UseCases.OrderUseCases.CreateOrderUseCase;

internal class CreateOrderRequestHandler(Cart cart, IProductService productService, IDbService dbService)
	: IRequestHandler<CreateOrderRequest>
{
	public async Task Handle(CreateOrderRequest request, CancellationToken cancellationToken)
	{
		var products = cart.Items.Values.ToList();

		if (!products.Any()) 
		{
			throw new OrderException("Your cart is empty");
		}

		var orderProducts = await productService.TakeProductsIfSufficientQuantityAsync(products);
		if(orderProducts is null)
		{
			throw new OrderException("Not enought products in stock");
		}

		var order = new Order { 
			CreatedDate = DateTime.Now,
			Products = orderProducts.ToList(),
			TotalPrice = cart.TotalCost,
			UserId = request.userId
		};

		await dbService.CreateOrderAsync(order);

		cart.ClearAll();
	}
}
