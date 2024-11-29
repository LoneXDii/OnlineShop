using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

internal class GetCategoryAttributesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpecificationFactory specificationFactory)
    : IRequestHandler<GetCategoryAttributesRequest, List<CategoryDTO>>
{
    public async Task<List<CategoryDTO>> Handle(GetCategoryAttributesRequest request, CancellationToken cancellationToken)
    {
        var specification = specificationFactory.CreateSpecification<Category>();
        specification.Criteries.Add(category => category.ParentId == request.categoryId);

        var attributes = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var attributesDto = mapper.Map<List<CategoryDTO>>(attributes);

        return attributesDto;

    }
}
