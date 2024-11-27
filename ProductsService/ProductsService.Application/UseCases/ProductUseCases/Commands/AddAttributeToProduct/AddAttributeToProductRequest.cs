using MediatR;
using ProductsService.Application.DTO;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddAttributeToProduct;

public sealed record AddAttributeToProductRequest(AddProductAttributeDTO attributeValue) : IRequest { }
