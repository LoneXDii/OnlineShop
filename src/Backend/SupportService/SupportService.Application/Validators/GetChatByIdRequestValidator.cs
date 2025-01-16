using FluentValidation;
using SupportService.Application.UseCases.GetChatById;

namespace SupportService.Application.Validators;

public class GetChatByIdRequestValidator : AbstractValidator<GetChatByIdRequest>
{
    public GetChatByIdRequestValidator()
    {
        RuleFor(req => req.UserId)
            .NotEmpty()
            .WithMessage("Wrong user Id")
            .When(req => req.UserId is not null);

        RuleFor(req => req.ChatId)
            .GreaterThan(0)
            .WithMessage("Wrong chat Id");
    }
}
