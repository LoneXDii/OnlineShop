using FluentValidation;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;

namespace ProductsService.Application.Validators.Discounts;

public class AddDiscountRequestValidator : AbstractValidator<AddDiscountRequest>
{
    public AddDiscountRequestValidator()
    {
        RuleFor(req => req.ProductId)
            .GreaterThan(0)
            .WithMessage("Wrong discount id");

        RuleFor(req => req.Percent)
            .GreaterThan(0)
            .LessThan(100)
            .WithMessage("Wrong discount percent");

        RuleFor(req => req.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Wrong discount start time");

        RuleFor(req => req)
            .Must(req => req.EndDate > req.StartDate)
            .WithMessage("Wrong discount end time");
    }
}
