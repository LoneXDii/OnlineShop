using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

//This class will be replaced by ProductService after gRPC implementation
internal class FakeProductService : IProductService
{
	private List<Product> _products;

	public FakeProductService()
	{
		_products = new List<Product>
		{
			new Product
			{
				Id = 1,
				Name = "Product1",
				Price = 99.99,
				Quantity = 10,
				ImageUrl = "Image1"
			},

			new Product
			{
				Id = 2,
				Name = "Product2",
				Price = 59.99,
				Quantity = 3,
				ImageUrl = "Image2"
			},

			new Product
			{
				Id = 3,
				Name = "Product3",
				Price = 12.49,
				Quantity = 14,
				ImageUrl = "Image3"
			},

			new Product
			{
				Id = 4,
				Name = "Product4",
				Price = 49.99,
				Quantity = 4,
				ImageUrl = "Image4"
			},

			new Product
			{
				Id = 5,
				Name = "Product5",
				Price = 25.00,
				Quantity = 20,
				ImageUrl = "Image5"
			},

			new Product
			{
				Id = 6,
				Name = "Product6",
				Price = 15.75,
				Quantity = 8,
				ImageUrl = "Image6"
			},

			new Product
			{
				Id = 7,
				Name = "Product7",
				Price = 89.99,
				Quantity = 2,
				ImageUrl = "Image7"
			},

			new Product
			{
				Id = 8,
				Name = "Product8",
				Price = 34.99,
				Quantity = 6,
				ImageUrl = "Image8"
			},

			new Product
			{
				Id = 9,
				Name = "Product9",
				Price = 19.99,
				Quantity = 12,
				ImageUrl = "Image9"
			},

			new Product
			{
				Id = 10,
				Name = "Product10",
				Price = 74.99,
				Quantity = 5,
				ImageUrl = "Image10"
			},
		};
	}

	public async Task<Product?> GetByIdIfSufficientQuantityAsync(int id, int quantity)
	{
		var product = _products.FirstOrDefault(p => p.Id == id);
		Product? returnProduct = null;

		if(product?.Quantity >= quantity)
		{
			returnProduct = new Product
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				ImageUrl = product.ImageUrl,
				Quantity = quantity
			};
		}
		await Task.Delay(100);

		return returnProduct;
	}

	public async Task<IEnumerable<Product>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<Product> products)
	{
		var retProducts = new List<Product>();
		foreach (var product in products)
		{
			var dbProduct = _products.FirstOrDefault(p => p.Id == product.Id);
			if (dbProduct?.Quantity >= product.Quantity)
			{
				dbProduct.Quantity -= product.Quantity;

				var retProduct = new Product
				{
					Id = product.Id,
					Name = product.Name,
					Price = product.Price,
					ImageUrl = product.ImageUrl,
					Quantity = product.Quantity
				};
				retProducts.Add(retProduct);
			}
			else
			{
				return null;
			}
		}

		await Task.Delay(100);

		return retProducts;
	}

	public async Task ReturnProductsAsync(IEnumerable<Product> products)
	{
		foreach (var product in products)
		{
			var dbProduct = _products.FirstOrDefault(p => p.Id == product.Id);
			if(dbProduct is not null)
			{
				dbProduct.Quantity += product.Quantity;
			}
		}

		await Task.Delay(100);
	}
}
