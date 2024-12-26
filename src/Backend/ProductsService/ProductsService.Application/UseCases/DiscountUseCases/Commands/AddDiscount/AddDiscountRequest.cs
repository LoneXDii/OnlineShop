using MediatR;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;

public sealed record AddDiscountRequest(int ProductId, DateTime StartDate, DateTime EndDate, int Percent) : IRequest { }
