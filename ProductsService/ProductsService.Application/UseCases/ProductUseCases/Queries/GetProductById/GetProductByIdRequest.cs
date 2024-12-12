using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.GetProductById;

public sealed record GetProductByIdRequest(int ProductId) : IRequest<ResponseProductDTO> { }
