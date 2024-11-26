using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Categoies;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

internal class GetUniqueCategoryAttributesValuesRequestHandler (IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUniqueCategoryAttributesValuesRequest, List<CategoryAttributesValuesDTO>> 
{
    public async Task<List<CategoryAttributesValuesDTO>> Handle(GetUniqueCategoryAttributesValuesRequest request, CancellationToken cancellationToken)
    {
        var specification = new CategoryAttributesValuesSpecification(request.categoryId);

        var categoriesValues = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var data = mapper.Map<List<CategoryAttributesValuesDTO>>(categoriesValues);

        return data;
    }
}
