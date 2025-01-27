using FluentValidation;
using UserService.BLL.UseCases.UserUseCases.ResetPasswordUseCase;

namespace UserService.BLL.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(req => req.Code)
            .NotEmpty()
            .Length(6, 6)
            .WithMessage("Wrong code");

        RuleFor(req => req.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long");
    }
}
