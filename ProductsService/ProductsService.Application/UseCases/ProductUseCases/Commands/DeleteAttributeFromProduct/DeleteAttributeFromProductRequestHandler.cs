using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        var specification = new ProductIncludeCategoriesSpecification();

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
