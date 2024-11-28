using AutoMapper;
using MediatR;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddCategory;

internal class AddCategoryRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<AddCategoryRequest>
{
    public async Task Handle(AddCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = mapper.Map<Category>(request);

        await unitOfWork.CategoryCommandRepository.AddAsync(category, cancellationToken);

        await unitOfWork.SaveAllAsync(cancellationToken);
    }
}
