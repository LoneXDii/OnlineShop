using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications.Categories;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryAttributes;

internal class GetCategoryAttributesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetCategoryAttributesRequest, List<ResponseCategoryDTO>>
{
    public async Task<List<ResponseCategoryDTO>> Handle(GetCategoryAttributesRequest request, CancellationToken cancellationToken)
    {
        var specification = new ParentCategorySpecification(request.CategoryId);

        var attributes = await unitOfWork.CategoryQueryRepository.ListAsync(specification, cancellationToken);

        return mapper.Map<List<ResponseCategoryDTO>>(attributes);
    }
}
