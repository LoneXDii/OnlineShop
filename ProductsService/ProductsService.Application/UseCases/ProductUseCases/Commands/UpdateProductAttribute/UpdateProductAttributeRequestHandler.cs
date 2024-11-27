using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

internal class UpdateProductAttributeRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductAttributeRequest>
{
    public async Task Handle(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
    {
        //var productAttribute = await unitOfWork.ProductAttributeQueryRepository.GetByIdAsync(request.ProductAttributeId, cancellationToken);

        //if (productAttribute is null)
        //{
        //    throw new NotFoundException("No such attribute");
        //}

        //productAttribute.Value = request.Value;

        //await unitOfWork.ProductAttributeCommandRepository.UpdateAsync(productAttribute, cancellationToken);

        //await unitOfWork.SaveAllAsync(cancellationToken);
        throw new NotImplementedException();
    }
}
