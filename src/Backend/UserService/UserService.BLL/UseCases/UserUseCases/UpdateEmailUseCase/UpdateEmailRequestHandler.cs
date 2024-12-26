using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Web;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;

internal class UpdateEmailRequestHandler(UserManager<AppUser> userManager, IEmailService emailService,
    ILogger<UpdateEmailRequestHandler> logger)
    : IRequestHandler<UpdateEmailRequest>
{
    public async Task Handle(UpdateEmailRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update email for user with id: {request.userId}");

        var userWithNewEmail = await userManager.FindByEmailAsync(request.newEmail);

        if (userWithNewEmail is not null)
        {
            logger.LogError($"Cannot set email: {request.newEmail} for user with id: {request.userId}, this email is already in use");

            throw new BadRequestException("This email is already in use");
        }

        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            logger.LogError($"User with id: {request.userId} not found");

            throw new NotFoundException("No such user");
        }

        BackgroundJob.Schedule(() => ReturnOldEmailAsync(user.Email, request.newEmail), TimeSpan.FromHours(1));

        user.Email = request.newEmail;
        user.UserName = request.newEmail;
        user.EmailConfirmed = false;

        await userManager.UpdateAsync(user);

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationCodeAsync(user.Email, code));

        logger.LogInformation($"Email for user with id: {request.userId} successfully updated");
    }

    public async Task ReturnOldEmailAsync(string oldEmail, string newEmail)
    {
        var user = await userManager.FindByEmailAsync(newEmail);

        if (user.EmailConfirmed)
        {
            return;
        }

        user.Email = oldEmail;
        user.EmailConfirmed = true;

        await userManager.UpdateAsync(user);

        await emailService.SendEmailNotChangedNotificationAsync(oldEmail, newEmail);
    }
}
