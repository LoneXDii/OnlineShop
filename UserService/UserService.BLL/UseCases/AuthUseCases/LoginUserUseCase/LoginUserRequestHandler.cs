using Microsoft.AspNetCore.Identity;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Entities;
using MediatR;
using UserService.DAL.Models;
using UserService.BLL.Exceptions;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.AuthUseCases.LoginUserUseCase;

internal class LoginUserRequestHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, 
    ITokenService tokenService, ILogger<LoginUserRequestHandler> logger)
    : IRequestHandler<LoginUserRequest, TokensDTO>
{
    public async Task<TokensDTO> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with email: {request.LoginModel.Email} trying to login");

        var signInResult = await signInManager.PasswordSignInAsync(request.LoginModel.Email, request.LoginModel.Password, false, false);

        if (!signInResult.Succeeded)
        {
            logger.LogError($"Cannot login user with email: {request.LoginModel.Email}, incorrect email or password");

            throw new BadRequestException("Incorrect email or password");
        }

        var user = await userManager.FindByEmailAsync(request.LoginModel.Email);

        if (!user!.EmailConfirmed)
        {
            logger.LogError($"Cannot login user with email: {request.LoginModel.Email}, email is not verified");

            throw new BadRequestException("Email is not verified");
        }

        var tokens = await tokenService.GetTokensAsync(user);

        logger.LogInformation($"User with id: {user.Id} and email: {user.Email} logged in");

        return tokens;
    }
}
