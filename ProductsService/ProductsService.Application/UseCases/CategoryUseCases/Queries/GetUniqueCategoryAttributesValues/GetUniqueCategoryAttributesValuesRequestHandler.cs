using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

internal class GetUniqueCategoryAttributesValuesRequestHandler (IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUniqueCategoryAttributesValuesRequest, List<CategoryAttributesValuesDTO>> 
{
    public async Task<List<CategoryAttributesValuesDTO>> Handle(GetUniqueCategoryAttributesValuesRequest request, CancellationToken cancellationToken)
    {
        var specification = new ParentCategorySpecification(request.CategoryId);

        var categoriesValues = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken,
            category => category.Children);

        return mapper.Map<List<CategoryAttributesValuesDTO>>(categoriesValues);
    }
}
