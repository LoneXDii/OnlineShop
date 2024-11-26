using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.ProductAttributes;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetUniqueCategoryAttributesValues;

internal class GetUniqueCategoryAttributesValuesRequestHandler (IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetUniqueCategoryAttributesValuesRequest, List<CategoryAttributesValuesDTO>> 
{
    public async Task<List<CategoryAttributesValuesDTO>> Handle(GetUniqueCategoryAttributesValuesRequest request, CancellationToken cancellationToken)
    {
        var specification = new CombinableSpecification<ProductAttribute>();
        specification = specification & new ProductAttributesCategorySpecification(request.categoryId);
        specification = specification & new UniqueProductAttributeValueSpecification();

        var productAttributes = await unitOfWork.ProductAttributeQueryRepository.ListAsync(specification, cancellationToken);

        var data = mapper.Map<List<CategoryAttributesValuesDTO>>(productAttributes);

        return data;
    }
}
