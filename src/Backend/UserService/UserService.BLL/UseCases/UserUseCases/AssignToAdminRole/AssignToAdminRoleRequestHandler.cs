using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.DAL.Entities;

namespace UserService.BLL.UseCases.UserUseCases.AssignToAdminRole;

internal class AssignToAdminRoleRequestHandler(UserManager<AppUser> userManager, 
    ILogger<AssignToAdminRoleRequestHandler> logger)
    : IRequestHandler<AssignToAdminRoleRequest>
{
    public async Task Handle(AssignToAdminRoleRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to make user with id: {request.UserId} an admin");

        var user = await userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            logger.LogError($"User with id={request.UserId} does not exists");

            throw new NotFoundException("No such user");
        }

        await userManager.AddToRoleAsync(user, "admin");
    }
}
