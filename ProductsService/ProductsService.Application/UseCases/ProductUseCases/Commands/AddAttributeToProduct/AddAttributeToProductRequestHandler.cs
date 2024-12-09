using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

internal class AddAttributeToProductRequestHandler(IUnitOfWork unitOfWork, ISpecificationFactory specificationFactory) 
    : IRequestHandler<AddAttributeToProductRequest>
{
    public async Task Handle(AddAttributeToProductRequest request, CancellationToken cancellationToken)
    {
        var attribute = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.AttributeId, cancellationToken);

        var specification = specificationFactory.CreateSpecification<Product>();
        specification.Includes.Add(product => product.Categories);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, specification, cancellationToken);

        if (product is null || attribute is null)
        {
            throw new BadRequestException("No such product or attribute");
        }

        if (product.Categories.Any(c => c.Id == attribute.Id))
        {
            throw new BadRequestException("Cant add existing product");
        }

        if (!product.Categories.Any(c => c.Id == attribute.ParentId))
        {
            throw new BadRequestException("No parent for attribute in this product");
        }

        unitOfWork.AttachInCommandContext(product);
        unitOfWork.AttachInCommandContext(attribute);

        product.Categories.Add(attribute);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
