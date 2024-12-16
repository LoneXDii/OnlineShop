using MediatR;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.ReturnProducts;

internal class ReturnProductsRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ReturnProductsRequest>
{
    public async Task Handle(ReturnProductsRequest request, CancellationToken cancellationToken)
    {
        var specification = new ProductsListBuIdsSpecification(request.Products.Keys);

        var products = await unitOfWork.ProductQueryRepository.ListAsync(specification, cancellationToken);

        foreach (var product in products)
        {
            product.Quantity += request.Products[product.Id];

            await unitOfWork.ProductCommandRepository.UpdateAsync(product, cancellationToken);
        }

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
