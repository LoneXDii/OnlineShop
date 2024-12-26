using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;

internal class ResendEmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, 
    ICacheService cache, ILogger<ResendEmailConfirmationCodeRequest> logger)
    : IRequestHandler<ResendEmailConfirmationCodeRequest>
{
   public async Task Handle(ResendEmailConfirmationCodeRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with email: {request.Email} asked for resending email confirmation code");

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            logger.LogError($"User with email: {request.Email} not found");

            throw new BadRequestException("No such user");
        }

        if (user.EmailConfirmed)
        {
            logger.LogError($"Email: {request.Email} already confirmed");

            throw new BadRequestException("Email already confirmed");
        }

        var code = await cache.SetEmailConfirmationCodeAsync(request.Email);

        BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationCodeAsync(request.Email, code));
    }
}
