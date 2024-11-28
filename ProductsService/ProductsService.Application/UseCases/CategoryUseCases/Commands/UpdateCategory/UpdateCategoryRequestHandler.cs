using AutoMapper;
using MediatR;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.UpdateCategory;

internal class UpdateCategoryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateCategoryRequest>
{
    public async Task Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new BadRequestException("No such category");
        }

        mapper.Map(request, category);

        await unitOfWork.CategoryCommandRepository.UpdateAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
