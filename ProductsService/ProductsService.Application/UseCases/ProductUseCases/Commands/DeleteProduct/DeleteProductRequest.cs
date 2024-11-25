using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteProduct;

public sealed record DeleteProductRequest(int productId) : IRequest {}
