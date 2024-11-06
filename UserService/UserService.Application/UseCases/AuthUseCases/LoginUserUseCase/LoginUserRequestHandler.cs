using Microsoft.AspNetCore.Identity;
using UserService.Infrastructure.Services.Authentication;
using UserService.Domain.Entities;
using MediatR;
using UserService.Infrastructure.Models;
using UserService.Domain.Exceptions;

namespace UserService.Application.UseCases.AuthUseCases.LoginUserUseCase;

internal class LoginUserRequestHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ITokenService tokenService)
	: IRequestHandler<LoginUserRequest, TokensDTO>
{
	public async Task<TokensDTO> Handle(LoginUserRequest request, CancellationToken cancellationToken)
	{
		var signInResult = await signInManager.PasswordSignInAsync(request.LoginModel.Email, request.LoginModel.Password, false, false);

		if (!signInResult.Succeeded)
		{
			throw new LoginException("Incorrect email or password");
		}

		var user = await userManager.FindByEmailAsync(request.LoginModel.Email);

		if (!user!.EmailConfirmed)
		{
			throw new LoginException("Email is not verified");
		}

		var tokens = await tokenService.GetTokensAsync(user);
		return tokens;
	}
}
