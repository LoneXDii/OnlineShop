using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

internal class UpdateProductAttributeRequestHandler(IUnitOfWork unitOfWork, ISpecificationFactory specificationFactory)
    : IRequestHandler<UpdateProductAttributeRequest>
{
    public async Task Handle(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
    {

        var newAttribute = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewAttributeId, cancellationToken);

        var specification = specificationFactory.CreateSpecification<Product>();
        specification.Includes.Add(product => product.Categories);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, specification, cancellationToken);

        var oldAttribute = product.Categories.FirstOrDefault(c => c.Id == request.OldAttributeId);

        if (newAttribute is null || oldAttribute is null || product is null) 
        {
            throw new BadRequestException("No entities with this isd");
        }

        if(newAttribute.ParentId != oldAttribute.ParentId)
        {
            throw new BadRequestException("Attributes must have the same parent");
        }

        if(product.Categories.Any(c => c.Id == newAttribute.Id))
        {
            throw new BadRequestException("Cant add existing attribute");
        }

        unitOfWork.AttachInCommandContext(newAttribute);
        unitOfWork.AttachInCommandContext(oldAttribute);
        unitOfWork.AttachInCommandContext(product);

        product.Categories.Remove(oldAttribute);
        product.Categories.Add(newAttribute);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
