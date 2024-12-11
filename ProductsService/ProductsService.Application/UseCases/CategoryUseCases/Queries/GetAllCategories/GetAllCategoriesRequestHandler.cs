using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetAllCategories;

internal class GetAllCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpecificationFactory specificationFactory)
    : IRequestHandler<GetAllCategoriesReguest, List<ResponseCategoryDTO>>
{
    public async Task<List<ResponseCategoryDTO>> Handle(GetAllCategoriesReguest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Category>();
        specification.Includes.Add(category => category.Children);
        specification.Criteries.Add(category => category.ParentId == null);

        var categories = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var categoriesDto = mapper.Map<List<ResponseCategoryDTO>>(categories);

        return categoriesDto;
    }
}
