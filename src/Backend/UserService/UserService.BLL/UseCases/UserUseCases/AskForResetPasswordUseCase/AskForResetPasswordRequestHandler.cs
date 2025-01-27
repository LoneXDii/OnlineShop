using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;

internal class AskForResetPasswordRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, ICacheService cacheService,
    ILogger<AskForResetPasswordRequestHandler> logger, IBackgroundJobProxy backgroundJob)
    : IRequestHandler<AskForResetPasswordRequest>
{
    public async Task Handle(AskForResetPasswordRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with email: {request.Email} asked for password reset");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogError($"No user with email: {request.Email}");

            throw new BadRequestException("No such user");
        }

        var code = await cacheService.SetResetPasswordCodeAsync(request.Email);

        backgroundJob.Enqueue(() => emailService.SendPasswordResetCodeAsync(request.Email, code));
    }
}
