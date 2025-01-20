using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

internal class UpdateProductAttributeRequestHandler(IUnitOfWork unitOfWork, ILogger<UpdateProductAttributeRequestHandler> logger)
    : IRequestHandler<UpdateProductAttributeRequest>
{
    public async Task Handle(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update attribute with id: {request.OldAttributeId} to id:{request.NewAttributeId} for product id: {request.ProductId}");

        var newAttributeValue = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, cancellationToken);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories);

        var oldAttributeValue = product.Categories.FirstOrDefault(c => c.Id == request.OldAttributeId);

        CheckDbData(oldAttributeValue, newAttributeValue, product);

        unitOfWork.AttachInCommandContext(newAttributeValue, oldAttributeValue, product);

        product.Categories.Remove(oldAttributeValue);
        product.Categories.Add(newAttributeValue);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Attribute of product id: {request.ProductId} successfully updated from id: {request.OldAttributeId} to id:{request.NewAttributeId}");
    }

    private void CheckDbData(Category? oldAttributeValue, Category? newAttributeValue, Product? product)
    {
        if (newAttributeValue is null || oldAttributeValue is null || product is null)
        {
            logger.LogError($"Any of attributes or products with this ids not found");

            throw new BadRequestException("No entities with this isd");
        }

        if (newAttributeValue.ParentId != oldAttributeValue.ParentId)
        {
            logger.LogError($"Parent of old attribute parentId: {oldAttributeValue.ParentId} is not match wit new attribute parentId: {newAttributeValue.ParentId}");

            throw new BadRequestException("Attributes must have the same parent");
        }

        if (product.Categories.Any(c => c.Id == newAttributeValue.Id))
        {
            logger.LogError($"Product with id: {product.Id} already have an attribute with id: {newAttributeValue.Id}");

            throw new BadRequestException("Сan't add an existing attribute");
        }
    }
}
