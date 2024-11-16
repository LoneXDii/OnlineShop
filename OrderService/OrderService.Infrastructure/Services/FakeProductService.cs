using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Services;

//This class will be replaced by ProductService after gRPC implementation
internal class FakeProductService : IProductService
{
    public List<ProductEntity> _products;

    public FakeProductService()
    {
        _products = new List<ProductEntity>
        {
            new ProductEntity
            {
                Id = 1,
                Name = "Product1",
                Price = 99.99,
                Quantity = 10,
                ImageUrl = "Image1",
                PriceId = "price_1QL9asCLnke0wpIT8ZGevnAn"
            },

            new ProductEntity
            {
                Id = 2,
                Name = "Product2",
                Price = 59.99,
                Quantity = 3,
                ImageUrl = "Image2",
                PriceId = "price_1QL9asCLnke0wpITEms9kRvm"
            },

            new ProductEntity
            {
                Id = 3,
                Name = "Product3",
                Price = 12.49,
                Quantity = 14,
                ImageUrl = "Image3",
                PriceId = "price_1QL9atCLnke0wpITN3oF1dpC"
            },

            new ProductEntity
            {
                Id = 4,
                Name = "Product4",
                Price = 49.99,
                Quantity = 4,
                ImageUrl = "Image4",
                PriceId = "price_1QL9atCLnke0wpITqZMUJGP0"
            },

            new ProductEntity
            {
                Id = 5,
                Name = "Product5",
                Price = 25.00,
                Quantity = 20,
                ImageUrl = "Image5",
                PriceId = "price_1QL9auCLnke0wpITAY9ejpxO"
            },

            new ProductEntity
            {
                Id = 6,
                Name = "Product6",
                Price = 15.75,
                Quantity = 8,
                ImageUrl = "Image6",
                PriceId = "price_1QL9avCLnke0wpITAY1dv1ny"
            },

            new ProductEntity
            {
                Id = 7,
                Name = "Product7",
                Price = 89.99,
                Quantity = 2,
                ImageUrl = "Image7",
                PriceId = "price_1QL9avCLnke0wpITUgWjX96Q"
            },

            new ProductEntity
            {
                Id = 8,
                Name = "Product8",
                Price = 34.99,
                Quantity = 6,
                ImageUrl = "Image8",
                PriceId = "price_1QL9awCLnke0wpITzZSZ7Hep"
            },

            new ProductEntity
            {
                Id = 9,
                Name = "Product9",
                Price = 19.99,
                Quantity = 12,
                ImageUrl = "Image9",
                PriceId = "price_1QL9awCLnke0wpIT57T8zqvC"
            },

            new ProductEntity
            {
                Id = 10,
                Name = "Product10",
                Price = 74.99,
                Quantity = 5,
                ImageUrl = "Image10",
                PriceId = "price_1QL9axCLnke0wpITZKXdy4L2"
            },
        };
    }

    public async Task<ProductEntity?> GetByIdIfSufficientQuantityAsync(int id, int quantity)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        ProductEntity? returnProduct = null;

        if(product?.Quantity >= quantity)
        {
            returnProduct = new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Quantity = quantity,
                PriceId = product.PriceId
            };
        }

        //Just to make async method really async, in real product service previous logick will be placed in Products microservice
        //Real product service will use gRPC to access product data
        await Task.Delay(100);

        return returnProduct;
    }

    public async Task<IEnumerable<ProductEntity>?> TakeProductsIfSufficientQuantityAsync(IEnumerable<ProductEntity> products)
    {
        var retProducts = new List<ProductEntity>();

        foreach (var product in products)
        {
            var dbProduct = _products.FirstOrDefault(p => p.Id == product.Id);

            if (dbProduct?.Quantity >= product.Quantity)
            {
                dbProduct.Quantity -= product.Quantity;

                var retProduct = new ProductEntity
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Quantity = product.Quantity,
                    PriceId = product.PriceId
                };
                retProducts.Add(retProduct);
            }
            else
            {
                return null;
            }
        }

        //Just to make async method really async, in real product service previous logick will be placed in Products microservice
        //Real product service will use gRPC to access product data
        await Task.Delay(100);

        return retProducts;
    }

    public async Task ReturnProductsAsync(IEnumerable<ProductEntity> products)
    {
        foreach (var product in products)
        {
            var dbProduct = _products.FirstOrDefault(p => p.Id == product.Id);

            if(dbProduct is not null)
            {
                dbProduct.Quantity += product.Quantity;
            }
        }

        //Just to make async method really async, in real product service previous logick will be placed in Products microservice
        //Real product service will use gRPC to access product data
        await Task.Delay(100);
    }
}
