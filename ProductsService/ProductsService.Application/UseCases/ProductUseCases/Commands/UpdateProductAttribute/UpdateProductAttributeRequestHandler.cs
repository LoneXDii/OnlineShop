using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProductAttribute;

internal class UpdateProductAttributeRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductAttributeRequest>
{
    public async Task Handle(UpdateProductAttributeRequest request, CancellationToken cancellationToken)
    {
        var oldAttribute = await unitOfWork.CategoryProductQueryRepository.FirstOrDefaultAsync(cp => cp.ProductId == request.ProductId && cp.CategoryId == request.OldValueId,
            cancellationToken, cp => cp.Category);

        var newAttribute = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.NewValueId, cancellationToken);

        if (oldAttribute is null || newAttribute is null)
        {
            throw new BadRequestException("Wrong attributes ids");
        }

        if (newAttribute.ParentId != oldAttribute.Category.ParentId)
        {
            throw new BadRequestException("New value and old values must be from same attribute");
        }

        await unitOfWork.CategoryProductCommandRepository.DeleteAsync(oldAttribute, cancellationToken);

        await unitOfWork.CategoryProductCommandRepository.AddAsync(new CategoryProduct { CategoryId = newAttribute.Id, ProductId = request.ProductId }, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
