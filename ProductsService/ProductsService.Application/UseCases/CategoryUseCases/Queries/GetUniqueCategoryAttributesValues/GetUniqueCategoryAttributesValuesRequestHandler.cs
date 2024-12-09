using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

internal class GetUniqueCategoryAttributesValuesRequestHandler (IUnitOfWork unitOfWork, IMapper mapper, ISpecificationFactory specificationFactory)
    : IRequestHandler<GetUniqueCategoryAttributesValuesRequest, List<CategoryAttributesValuesDTO>> 
{
    public async Task<List<CategoryAttributesValuesDTO>> Handle(GetUniqueCategoryAttributesValuesRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Category>();
        specification.Criteries.Add(category => category.ParentId == request.categoryId);
        specification.Includes.Add(category => category.Children);

        var categoriesValues = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var data = mapper.Map<List<CategoryAttributesValuesDTO>>(categoriesValues);

        return data;
    }
}
