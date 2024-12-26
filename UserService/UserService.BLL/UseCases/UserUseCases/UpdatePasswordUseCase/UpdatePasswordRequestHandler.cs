using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.Authentication;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.UserUseCases.UpdatePasswordUseCase;

internal class UpdatePasswordRequestHandler(UserManager<AppUser> userManager, ITokenService tokenService,
    ILogger<UpdatePasswordRequestHandler> logger)
    : IRequestHandler<UpdatePasswordRequest, string>
{
    public async Task<string> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update password for user with id: {request.userId}");

        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            logger.LogError($"User with id: {request.userId} not found");

            throw new NotFoundException("No such user");
        }

        var changePasswordResult = await userManager.ChangePasswordAsync(user, request.updatePasswordDTO.OldPassword, request.updatePasswordDTO.NewPassword);

        if (!changePasswordResult.Succeeded)
        {
            var errors = new StringBuilder();

            foreach (var error in changePasswordResult.Errors)
            {
                errors.AppendLine(error.Description);
            }

            logger.LogError($"Cannot update password for user with id: {request.userId}, errors: {errors.ToString()}");

            throw new BadRequestException(errors.ToString());
        }

        await tokenService.InvalidateRefreshTokenAsync(user);

        var refreshToken = await tokenService.GetRefreshTokenAsync(user);

        logger.LogInformation($"Password for user with id: {request.userId} successfully updated");

        return refreshToken;
    }
}
