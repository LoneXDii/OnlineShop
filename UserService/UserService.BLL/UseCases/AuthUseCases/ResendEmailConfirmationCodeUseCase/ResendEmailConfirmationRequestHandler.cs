using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;

internal class ResendEmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService, ICacheService cache)
    : IRequestHandler<ResendEmailConfirmationCodeRequest>
{
   public async Task Handle(ResendEmailConfirmationCodeRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new BadRequestException("No such user");
        }

        if (user.EmailConfirmed)
        {
            throw new BadRequestException("Email already confirmed");
        }

        var code = await cache.SetEmailConfirmationCodeAsync(request.Email);

        BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationCodeAsync(request.Email, code));
    }
}
