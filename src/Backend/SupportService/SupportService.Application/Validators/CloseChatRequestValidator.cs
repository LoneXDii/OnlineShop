using FluentValidation;
using SupportService.Application.UseCases.CloseChat;

namespace SupportService.Application.Validators;

public class CloseChatRequestValidator : AbstractValidator<CloseChatRequest>
{
    public CloseChatRequestValidator()
    {
        RuleFor(req => req.UserId)
            .NotEmpty()
            .WithMessage("Wrong user Id");

        RuleFor(req => req.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat Id");
    }
}
