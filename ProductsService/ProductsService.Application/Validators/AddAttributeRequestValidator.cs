using FluentValidation;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;

namespace ProductsService.Application.Validators;

public class AddAttributeRequestValidator : AbstractValidator<AddAttributeRequest>
{
    public AddAttributeRequestValidator()
    {
        RuleFor(req => req.Name)
            .NotEmpty()
            .WithMessage("Wrong attribute name");

        RuleFor(req => req.ParentId)
            .GreaterThan(0)
            .WithMessage("Wrong parent id");
    }
}
