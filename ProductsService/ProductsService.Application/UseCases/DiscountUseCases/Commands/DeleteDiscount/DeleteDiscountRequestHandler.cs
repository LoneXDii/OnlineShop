using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

internal class DeleteDiscountRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDiscountRequest>
{
    public async Task Handle(DeleteDiscountRequest request, CancellationToken cancellationToken)
    {
        var discount = await unitOfWork.DiscountQueryRepository.GetByIdAsync(request.DiscountId, cancellationToken);

        if (discount is null)
        {
            throw new NotFoundException("No such discount");
        }

        await unitOfWork.DiscountCommandRepository.DeleteAsync(discount, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
