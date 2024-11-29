using MediatR;

namespace ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

public sealed record DeleteDiscountRequest(int DiscountId) : IRequest { }
