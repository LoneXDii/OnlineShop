using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

internal class UpdateProductAttributeRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductAttributeRequest>
{
    public async Task Handle(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
    {

        var newAttributeValue = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, cancellationToken);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories);

        var oldAttributeValue = product.Categories.FirstOrDefault(c => c.Id == request.OldAttributeId);

        if (newAttributeValue is null || oldAttributeValue is null || product is null)
        {
            throw new BadRequestException("No entities with this isd");
        }

        if (newAttributeValue.ParentId != oldAttributeValue.ParentId)
        {
            throw new BadRequestException("Attributes must have the same parent");
        }

        if (product.Categories.Any(c => c.Id == newAttributeValue.Id))
        {
            throw new BadRequestException("Сan't add an existing attribute");
        }

        unitOfWork.AttachInCommandContext(newAttributeValue, oldAttributeValue, product);

        product.Categories.Remove(oldAttributeValue);
        product.Categories.Add(newAttributeValue);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
