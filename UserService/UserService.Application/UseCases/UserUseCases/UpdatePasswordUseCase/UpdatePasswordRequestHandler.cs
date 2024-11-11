using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.Text.RegularExpressions;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Infrastructure.Services.Authentication;

namespace UserService.Application.UseCases.UserUseCases.UpdatePasswordUseCase;

internal class UpdatePasswordRequestHandler(UserManager<AppUser> userManager, ITokenService tokenService)
	: IRequestHandler<UpdatePasswordRequest, string>
{
	public async Task<string> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByIdAsync(request.userId);
		if (user is null)
		{
			throw new NotFoundException("No such user");
		}

		var changePasswordResult = await userManager.ChangePasswordAsync(user, request.updatePasswordDTO.OldPassword, request.updatePasswordDTO.NewPassword);

		if (!changePasswordResult.Succeeded)
		{
			var errors = new StringBuilder();
			foreach (var error in changePasswordResult.Errors)
			{
				errors.AppendLine(error.Description);
			}
			throw new BadRequestException(errors.ToString());
		}

		await tokenService.InvalidateRefreshTokenAsync(user);
		var refreshToken = await tokenService.GetRefreshTokenAsync(user);
		return refreshToken;
	}
}
