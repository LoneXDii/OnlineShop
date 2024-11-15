using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.Authentication;

namespace UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;

internal class LogoutUserRequestHandler(UserManager<AppUser> userManager, ITokenService tokenService)
	: IRequestHandler<LogoutUserRequest>
{
	public async Task Handle(LogoutUserRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByIdAsync(request.userId);

		if(user is null)
		{
			throw new NotFoundException("No such user");
		}

		await tokenService.InvalidateRefreshTokenAsync(user);
	}
}
