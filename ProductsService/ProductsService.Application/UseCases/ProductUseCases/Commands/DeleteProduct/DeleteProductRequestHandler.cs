using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.BlobStorage;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;

internal class DeleteProductRequestHandler(IUnitOfWork unitOfWork, IBlobService blobService, 
    ILogger<DeleteProductRequestHandler> logger)
    : IRequestHandler<DeleteProductRequest>
{
    public async Task Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to delete product with id: {request.ProductId}");

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if(product is null)
        {
            logger.LogError($"Product with id: {request.ProductId} not found");

            throw new NotFoundException("No such product");
        }

        if(product.ImageUrl is not null)
        {
            await blobService.DeleteAsync(product.ImageUrl);
        }

        await unitOfWork.ProductCommandRepository.DeleteAsync(product, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Product with id: {request.ProductId} succesfully deleted");
    }
}
