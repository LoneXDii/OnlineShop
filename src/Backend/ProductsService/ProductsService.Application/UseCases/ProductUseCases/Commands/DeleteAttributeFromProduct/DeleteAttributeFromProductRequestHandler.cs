using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork, 
    ILogger<DeleteAttributeFromProductRequestHandler> logger)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to delete attribute with id: {request.AttributeId} from product with id: {request.ProductId}");

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories);

        var attribute = product?.Categories?.FirstOrDefault(c => c.Id == request.AttributeId);

        if (product is null || attribute is null)
        {
            logger.LogError($"Product with id: {request.ProductId} or attribute with id: {request.AttributeId} not found");

            throw new BadRequestException("No such product or attribute of this product");
        }

        unitOfWork.AttachInCommandContext(product);

        product.Categories.Remove(attribute);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Attribute with id: {request.AttributeId} succesfully deleted from product with id: {request.ProductId}");
    }
}
