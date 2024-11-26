using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;

internal class GetAllCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllCategoriesReguest, List<CategoryDTO>>
{
    public async Task<List<CategoryDTO>> Handle(GetAllCategoriesReguest request, CancellationToken cancellationToken)
    {
        var specification = new ParentCategoriesSpecification();

        var categories = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var categoriesDto = mapper.Map<List<CategoryDTO>>(categories);

        return categoriesDto;
    }
}
