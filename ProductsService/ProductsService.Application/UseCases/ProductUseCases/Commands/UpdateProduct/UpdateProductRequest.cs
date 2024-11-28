using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.UpdateProduct;

public sealed record UpdateProductRequest(int Id, string? Name, double? Price, int? Quantity, IFormFile? Image) 
	: IRequest {}
