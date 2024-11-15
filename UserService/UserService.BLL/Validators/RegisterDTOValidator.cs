using FluentValidation;
using UserService.BLL.DTO;

namespace UserService.BLL.Validators;

public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
{
    public RegisterDTOValidator()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .Matches(@"^\S+@\S+\.\S+$")
            .WithMessage("Incorrect email adress");

        RuleFor(u => u.Password)
            .NotEmpty()
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long");
    }
}
