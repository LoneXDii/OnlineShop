using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using Hangfire;
using Microsoft.Extensions.Logging;
using UserService.BLL.Proxy;

namespace UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;

internal class EmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, 
    ICacheService cacheService, ILogger<EmailConfirmationRequestHandler> logger, IBackgroundJobProxy backgroundJob)
    : IRequestHandler<EmailConfirmationRequest>
{
    public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with email: {request.email} trying to confirm email");

        var user = await userManager.FindByEmailAsync(request.email);

        if (user is null)
        {
            logger.LogError($"No user with email: {request.email} found");

            throw new NotFoundException("No user with such email");
        }

        var email = await cacheService.GetEmailByCodeAsync(request.code);

        if(user.Email == email)
        {
            user.EmailConfirmed = true;

            await userManager.UpdateAsync(user);

            logger.LogInformation($"Email: {email} succesfully confirmed");
        }
        else
        {
            logger.LogError($"Wrong confirmation code for emial: {request.email}");

            throw new BadRequestException($"Wrong code");
        }

        backgroundJob.Enqueue(() => emailService.SendEmailConfirmationSucceededNotificationAsync(user.Email));
    }
}
