using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.UserUseCases.ResetPaswordUseCase;

internal class ResetPasswordRequestHandler(UserManager<AppUser> userManager, ICacheService cacheService, 
    IEmailService emailService, ITokenService tokenService)
    : IRequestHandler<ResetPasswordRequest>
{
    public async Task Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var email = await cacheService.GetEmailByResetPasswordCodeAsync(request.Code);

        if (email is null)
        {
            throw new BadRequestException("No such code");
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            throw new BadRequestException("No such user");
        }

        await userManager.RemovePasswordAsync(user);

        await userManager.AddPasswordAsync(user, request.Password);

        await emailService.SendPasswordResetSucceededNotificationAsync(email);

        await tokenService.InvalidateRefreshTokenAsync(user);
    }
}
