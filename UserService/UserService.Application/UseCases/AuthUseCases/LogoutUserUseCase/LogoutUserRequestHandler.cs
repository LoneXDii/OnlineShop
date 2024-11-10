using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Infrastructure.Services.Authentication;

namespace UserService.Application.UseCases.AuthUseCases.LogoutUserUseCase;

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
