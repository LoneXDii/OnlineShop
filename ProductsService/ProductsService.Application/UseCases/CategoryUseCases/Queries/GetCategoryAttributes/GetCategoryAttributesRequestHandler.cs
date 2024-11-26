using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Attributes;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

internal class GetCategoryAttributesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetCategoryAttributesRequest, List<AttributeDTO>>
{
    public async Task<List<AttributeDTO>> Handle(GetCategoryAttributesRequest request, CancellationToken cancellationToken)
    {
        var specification = new AttributeCategorySpecification(request.categoryId);

        var attributes = await unitOfWork.AttributeQueryRepository.ListAsync(specification, cancellationToken);

        var attributesDto = mapper.Map<List<AttributeDTO>>(attributes);

        return attributesDto;
    }
}
