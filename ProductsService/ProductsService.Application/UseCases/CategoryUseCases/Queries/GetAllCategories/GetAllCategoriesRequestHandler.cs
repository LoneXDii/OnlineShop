using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;

internal class GetAllCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllCategoriesReguest, List<ResponseCategoryDTO>>
{
    public async Task<List<ResponseCategoryDTO>> Handle(GetAllCategoriesReguest request, CancellationToken cancellationToken)
    {
        var specification = new TopLevelCategorySpecification();

        var categories = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken,
            category => category.Children);

        return mapper.Map<List<ResponseCategoryDTO>>(categories);
    }
}
