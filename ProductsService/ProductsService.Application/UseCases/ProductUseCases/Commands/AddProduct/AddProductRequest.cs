using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

public sealed record AddProductRequest(PostProductDTO product) : IRequest { }
