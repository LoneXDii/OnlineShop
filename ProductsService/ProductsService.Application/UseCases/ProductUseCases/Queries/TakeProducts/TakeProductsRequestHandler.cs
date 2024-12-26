using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using System.Text;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.TakeProducts;

internal class TakeProductsRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<TakeProductsRequest, IEnumerable<Product>>
{
    public async Task<IEnumerable<Product>> Handle(TakeProductsRequest request, CancellationToken cancellationToken)
    {
        var specification = new ProductsListBuIdsSpecification(request.Products.Keys);

        var products = await unitOfWork.ProductQueryRepository.ListAsync(specification, cancellationToken, p => p.Discount);

        await UpdateProductsQuantityAsync(products, request.Products, cancellationToken);

        foreach (var product in products)
        {
            product.Quantity = request.Products[product.Id];
        }

        return products;
    }

    private async Task UpdateProductsQuantityAsync(IEnumerable<Product> products, IDictionary<int, int> requestedProducts,
        CancellationToken cancellationToken)
    {
        var errors = new StringBuilder();

        foreach (var product in products)
        {
            var quantity = requestedProducts[product.Id];

            if (product.Quantity < quantity)
            {
                errors.AppendLine($"Quantity of product with id={product.Id} is too low");
                continue;
            }

            product.Quantity -= quantity;

            await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);
        }

        if (errors.Length > 0)
        {
            throw new BadRequestException(errors.ToString());
        }

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
