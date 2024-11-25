using MediatR;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

internal class DeleteAttributeFromProductRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteAttributeFromProductRequest>
{
    public async Task Handle(DeleteAttributeFromProductRequest request, CancellationToken cancellationToken)
    {
        await unitOfWork.ProductAttributeCommandRepository.DeleteByIdAsync(request.productAttributeId, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
