using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries;

internal class GetAllCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetAllCategoriesReguest, List<CategoryDTO>>
{
    public async Task<List<CategoryDTO>> Handle(GetAllCategoriesReguest request, CancellationToken cancellationToken)
    {
        var categories = await unitOfWork.CategoryQueryRepository.ListAllAsync(cancellationToken);

        var categoriesDto = mapper.Map<List<CategoryDTO>>(categories);

        return categoriesDto;
    }
}
