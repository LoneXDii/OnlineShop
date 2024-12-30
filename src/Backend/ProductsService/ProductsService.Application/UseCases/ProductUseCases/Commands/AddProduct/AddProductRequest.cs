using MediatR;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.UseCases.ProductUseCases.Commands.AddProduct;

public sealed record AddProductRequest(string Name, double Price, int Quantity, Stream? Image, string? ImageContentType ,int[] Attributes) 
    : IRequest { }
