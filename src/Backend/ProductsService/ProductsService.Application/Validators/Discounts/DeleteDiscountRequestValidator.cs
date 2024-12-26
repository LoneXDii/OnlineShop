using FluentValidation;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

namespace ProductsService.Application.Validators.Discounts;

public class DeleteDiscountRequestValidator : AbstractValidator<DeleteDiscountRequest>
{
    public DeleteDiscountRequestValidator()
    {
        RuleFor(req => req.DiscountId)
            .GreaterThan(0)
            .WithMessage("Wrong discount id");
    }
}
