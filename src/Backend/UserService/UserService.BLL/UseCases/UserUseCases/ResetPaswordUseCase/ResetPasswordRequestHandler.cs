using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.UserUseCases.ResetPaswordUseCase;

internal class ResetPasswordRequestHandler(UserManager<AppUser> userManager, ICacheService cacheService, 
    IEmailService emailService, ITokenService tokenService, ILogger<ResetPasswordRequestHandler> logger)
    : IRequestHandler<ResetPasswordRequest>
{
    public async Task Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to reset password");

        var email = await cacheService.GetEmailByResetPasswordCodeAsync(request.Code);

        if (email is null)
        {
            logger.LogError($"Wrong password reset code");

            throw new BadRequestException("No such code");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            logger.LogError($"No user with email: {email}");

            throw new BadRequestException("No such user");
        }

        await userManager.RemovePasswordAsync(user);

        await userManager.AddPasswordAsync(user, request.Password);

        BackgroundJob.Enqueue(() => emailService.SendPasswordResetSucceededNotificationAsync(email));

        await tokenService.InvalidateRefreshTokenAsync(user);

        logger.LogInformation($"Password was reset successfully");
    }
}
