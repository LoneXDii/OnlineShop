using MediatR;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.DeleteAttributeFromProduct;

public sealed record DeleteAttributeFromProductRequest(int productAttributeId) : IRequest { }
