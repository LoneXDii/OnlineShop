using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;

internal class DeleteCategoryRequestHandler(IUnitOfWork unitOfWork, ILogger<DeleteCategoryRequestHandler> logger)
    : IRequestHandler<DeleteCategoryRequest>
{
    public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to delete category with id: {request.categoryId}");

        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.categoryId, cancellationToken);

        if (category is null)
        {
            logger.LogError($"Category with id: {request.categoryId} not found");

            throw new NotFoundException("No such category");
        }

        await unitOfWork.CategoryCommandRepository.DeleteAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);

        logger.LogInformation($"Category with id: {request.categoryId} successfully deleted");
    }
}
