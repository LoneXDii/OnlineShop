using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductsService.Application.DTO;
using ProductsService.Application.Exceptions;
using ProductsService.Domain.Abstractions.Database;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;

internal class GetCategoryByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper, 
    ILogger<GetCategoryByIdRequestHandler> logger)
    : IRequestHandler<GetCategoryByIdRequest, ResponseCategoryDTO>
{ 
    public async Task<ResponseCategoryDTO> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.CategoryQueryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            logger.LogError($"Category with id: {request.CategoryId} does not exists");

            throw new NotFoundException("No such category");
        }

        return mapper.Map<ResponseCategoryDTO>(category);
    }
}
