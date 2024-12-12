using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductQueryRepository.GetByIdAsync(request.ProductId, cancellationToken,
            product => product.Categories);

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
