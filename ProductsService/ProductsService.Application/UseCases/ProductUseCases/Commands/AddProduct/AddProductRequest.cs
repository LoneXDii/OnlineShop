using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

public sealed record AddProductRequest(string Name, double Price, int Quantity, IFormFile? Image, params int[] Attributes) 
	: IRequest { }
