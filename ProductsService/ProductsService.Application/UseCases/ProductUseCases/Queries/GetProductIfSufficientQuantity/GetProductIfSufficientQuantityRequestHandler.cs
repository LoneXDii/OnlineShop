using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;

internal class GetProductIfSufficientQuantityRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductIfSufficientQuantityRequest, Product>
{
    public async Task<Product> Handle(GetProductIfSufficientQuantityRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, cancellationToken, p => p.Discount);

        if (product is null)
        {
            throw new BadRequestException("No such product");
        }

        if (product.Quantity < request.Quantity)
        {
            throw new BadRequestException("Product's quantity is too low");
        }

        product.Quantity = request.Quantity;

        return product;
    }
}
