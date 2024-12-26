using FluentValidation;
using OrderService.Application.DTO;

namespace OrderService.Application.Validators;

public class PaginationValidator : AbstractValidator<PaginationDTO>
{
    public PaginationValidator()
    {
        RuleFor(pagination => pagination.PageNo)
            .GreaterThan(0)
            .WithMessage("Wrong page number");

        RuleFor(pagination => pagination.PageSize)
            .GreaterThan(0)
            .WithMessage("Wrong page size");
    }
}
