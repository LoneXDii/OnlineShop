using FluentValidation;

namespace SupportService.Application.Validators;

public class ChatIdValidator : AbstractValidator<int>
{
    public ChatIdValidator()
    {
        RuleFor(id => id)
            .GreaterThan(0)
            .WithMessage("Wrong chat id");
    }
}
