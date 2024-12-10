using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;
using System.Text.Json;
using UserService.DAL.Services.TemporaryStorage;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Hangfire;

namespace UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;

internal class EmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, ICacheService cacheService)
    : IRequestHandler<EmailConfirmationRequest>
{
    public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.email);

        if (user is null)
        {
            throw new NotFoundException("No user with such email");
        }

        var email = await cacheService.GetEmailByCodeAsync(request.code);

        if(user.Email == email)
        {
            user.EmailConfirmed = true;

            await userManager.UpdateAsync(user);
        }
        else
        {
            throw new BadRequestException($"Wrong code");
        }

        BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationSucceededNotificationAsync(user.Email));
    }
}
