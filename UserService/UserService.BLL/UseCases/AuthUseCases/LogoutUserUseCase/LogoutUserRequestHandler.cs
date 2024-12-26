using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.Authentication;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.AuthUseCases.LogoutUserUseCase;

internal class LogoutUserRequestHandler(UserManager<AppUser> userManager, ITokenService tokenService,
    ILogger<LogoutUserRequestHandler> logger)
    : IRequestHandler<LogoutUserRequest>
{
    public async Task Handle(LogoutUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with id: {request.userId} logging out");

        var user = await userManager.FindByIdAsync(request.userId);

        if(user is null)
        {
            logger.LogError($"User with id: {request.userId} not found");

            throw new NotFoundException("No such user");
        }
        logger.LogInformation($"User with id: {request.userId} logged out");

        await tokenService.InvalidateRefreshTokenAsync(user);
    }
}
