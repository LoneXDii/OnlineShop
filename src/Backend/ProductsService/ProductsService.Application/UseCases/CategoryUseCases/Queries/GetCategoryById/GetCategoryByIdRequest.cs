using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.CategoryUseCases.Queries.GetCategoryById;

public sealed record GetCategoryByIdRequest(int CategoryId) : IRequest<ResponseCategoryDTO> { }
