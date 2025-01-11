using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;

namespace UserService.BLL.UseCases.UserUseCases.RemoveUserFromAdmins;

internal class RemoveUserFromAdminsRequestHandler(UserManager<AppUser> userManager, 
    ILogger<RemoveUserFromAdminsRequestHandler> logger)
    : IRequestHandler<RemoveUserFromAdminsRequest>
{
    public async Task Handle(RemoveUserFromAdminsRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to remove user with id: {request.UserId} from admins");

        var user = await userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            logger.LogError($"User with id={request.UserId} does not exists");

            throw new NotFoundException("No such user");
        }

        await userManager.RemoveFromRoleAsync(user, "admin");
    }
}
