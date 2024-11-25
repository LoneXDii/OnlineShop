using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

public sealed record UpdateProductRequest(UpdateProductDTO productDTO) : IRequest {}
