using FluentValidation;
using UserService.Application.DTO;

namespace UserService.Application.Validators;

public class UpdatePasswordValidator : AbstractValidator<UpdatePasswordDTO>
{
	public UpdatePasswordValidator()
	{
		RuleFor(changePasswordDTO => changePasswordDTO.NewPassword)
			.NotEmpty()
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
			.WithMessage("Password must contains lower and uppercase letters, at least 1 digit and special symbol and be at least 8 symbols long");
	}
}
