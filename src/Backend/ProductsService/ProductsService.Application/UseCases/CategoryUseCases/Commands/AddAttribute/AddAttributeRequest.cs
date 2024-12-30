using MediatR;

namespace ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;

public sealed record AddAttributeRequest(int ParentId, string Name) : IRequest { }
