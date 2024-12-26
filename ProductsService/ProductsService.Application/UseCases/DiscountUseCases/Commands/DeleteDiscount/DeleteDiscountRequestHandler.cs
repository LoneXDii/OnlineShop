using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

internal class DeleteDiscountRequestHandler(IUnitOfWork unitOfWork, ILogger<DeleteDiscountRequestHandler> logger)
    : IRequestHandler<DeleteDiscountRequest>
{
    public async Task Handle(DeleteDiscountRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to delete discount with id: {request.DiscountId}");

        var discount = await unitOfWork.DiscountQueryRepository.GetByIdAsync(request.DiscountId, cancellationToken);

        if (discount is null)
        {
            logger.LogError($"Discount with id: {request.DiscountId} not found");

            throw new NotFoundException("No such discount");
        }

        await unitOfWork.DiscountCommandRepository.DeleteAsync(discount, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Discount with id: {request.DiscountId} successfully deleted");
    }
}
