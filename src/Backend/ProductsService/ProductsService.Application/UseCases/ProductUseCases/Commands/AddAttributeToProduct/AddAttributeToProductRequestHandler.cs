using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

internal class AddAttributeToProductRequestHandler(IUnitOfWork unitOfWork, 
    ILogger<AddAttributeToProductRequestHandler> logger) 
    : IRequestHandler<AddAttributeToProductRequest>
{
    public async Task Handle(AddAttributeToProductRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to add attribute with id: {request.AttributeId} to product with id: {request.ProductId}");

        var attribute = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, cancellationToken);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken, 
            product => product.Categories);

        if (product is null || attribute is null)
        {
            logger.LogError($"Product with id: {request.ProductId} or attribute with id: {request.AttributeId} not found");

            throw new BadRequestException("No such product or attribute");
        }

        if (product.Categories.Any(c => c.Id == attribute.Id))
        {
            logger.LogError($"Product with id: {product.Id} already has an attribute with id {attribute.Id}");

            throw new BadRequestException("This product already has such an attribute");
        }

        if (!product.Categories.Any(c => c.Id == attribute.ParentId))
        {
            logger.LogError($"No parent with id: {attribute.ParentId} found in product with id: {product.Id} for an attribute with id {attribute.Id}");

            throw new BadRequestException("No parent for this attribute in this product");
        }

        unitOfWork.AttachInCommandContext(product, attribute);

        product.Categories.Add(attribute);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Attribute with id: {request.AttributeId} succesfully added to product with id: {request.ProductId}");
    }
}
