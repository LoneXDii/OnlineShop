using FluentValidation;
using SupportService.Application.UseCases.GetChatMessages;

namespace SupportService.Application.Validators;

public class GetChatMessagesRequestValidator : AbstractValidator<GetChatMessagesRequest>
{
    public GetChatMessagesRequestValidator()
    {
        RuleFor(req => req.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat id");
    }
}
