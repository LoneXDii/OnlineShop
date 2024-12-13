using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Web;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;
using Hangfire;

namespace UserService.BLL.UseCases.UserUseCases.UpdateEmailUseCase;

internal class UpdateEmailRequestHandler(UserManager<AppUser> userManager, IEmailService emailService)
    : IRequestHandler<UpdateEmailRequest>
{
    public async Task Handle(UpdateEmailRequest request, CancellationToken cancellationToken)
    {
        var userWithNewEmail = await userManager.FindByEmailAsync(request.newEmail);

        if (userWithNewEmail is not null)
        {
            throw new BadRequestException("This email is already in use");
        }

        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            throw new NotFoundException("No such user");
        }

        BackgroundJob.Schedule(() => ReturnOldEmailAsync(user.Email, request.newEmail), TimeSpan.FromHours(1));

        user.Email = request.newEmail;
        user.UserName = request.newEmail;
        user.EmailConfirmed = false;

        await userManager.UpdateAsync(user);

        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

        BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationCodeAsync(user.Email, code));
    }

    public async Task ReturnOldEmailAsync(string oldEmail, string newEmail)
    {
        var user = await userManager.FindByEmailAsync(newEmail);

        if (!user.EmailConfirmed)
        {
            user.Email = oldEmail;
            user.EmailConfirmed = true;

            await userManager.UpdateAsync(user);

            await emailService.SendEmailNotChangedNotificationAsync(oldEmail, newEmail);
        }
    }
}
