using FluentValidation;
using SupportService.Application.UseCases.GetUserChats;

namespace SupportService.Application.Validators;

public class GetUserChatsRequestValidator : AbstractValidator<GetUserChatsRequest>
{
    public GetUserChatsRequestValidator()
    {
        RuleFor(req => req.UserId)
            .NotEmpty()
            .WithMessage("Wrong user Id");
    }
}
