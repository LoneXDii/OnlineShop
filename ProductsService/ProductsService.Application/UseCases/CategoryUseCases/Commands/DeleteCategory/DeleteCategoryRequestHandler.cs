using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.DeleteCategory;

internal class DeleteCategoryRequestHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryRequest>
{
    public async Task Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.categoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("No such category");
        }

        await unitOfWork.CategoryCommandRepository.DeleteAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
