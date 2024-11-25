using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        var productAttribute = await unitOfWork.ProductAttributeQueryRepository.GetByIdAsync(request.productAttributeId, cancellationToken);

        if (productAttribute is null)
        {
            throw new NotFoundException("No such product attribute");
        }

        await unitOfWork.ProductAttributeCommandRepository.DeleteAsync(productAttribute, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
