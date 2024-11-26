using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categoies;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

internal class GetCategoryAttributesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetCategoryAttributesRequest, List<CategoryDTO>>
{
    public async Task<List<CategoryDTO>> Handle(GetCategoryAttributesRequest request, CancellationToken cancellationToken)
    {
        var specification = new CategoryAttributesSpecification(request.categoryId);

        var attributes = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        var attributesDto = mapper.Map<List<CategoryDTO>>(attributes);

        return attributesDto;

    }
}
