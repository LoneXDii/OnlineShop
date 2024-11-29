using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork, ISpecificationFactory specificationFactory)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Product>();
        specification.Includes.Add(product => product.Categories);

        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, specification, cancellationToken);

        var attribute = product?.Categories?.FirstOrDefault(c => c.Id == request.AttributeId);

        if (product is null || attribute is null)
        {
            throw new BadRequestException("No such product or attribute of this product");
        }

        unitOfWork.AttachInCommandContext(product);

        product.Categories.Remove(attribute);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
