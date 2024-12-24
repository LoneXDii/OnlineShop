using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductIfSufficientQuantity;

internal class GetProductIfSufficientQuantityRequestHandler(IUnitOfWork unitOfWork, ILogger<GetProductIfSufficientQuantityRequestHandler> logger)
    : IRequestHandler<GetProductIfSufficientQuantityRequest, Product>
{
    public async Task<Product> Handle(GetProductIfSufficientQuantityRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.Id, cancellationToken, p => p.Discount);

        if (product is null)
        {
            logger.LogError($"No product with id: {request.Id} found");

            throw new BadRequestException("No such product");
        }

        if (product.Quantity < request.Quantity)
        {
            logger.LogError($"Not enough quantity: {request.Quantity} of product with id: {request.Id}");

            throw new BadRequestException("Product's quantity is too low");
        }

        product.Quantity = request.Quantity;

        return product;
    }
}
