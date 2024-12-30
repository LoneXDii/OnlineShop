using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.BlobStorage;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;

internal class UpdateUserRequestHandler(UserManager<AppUser> userManager, IMapper mapper, IBlobService blobService, 
    ILogger<UpdateUserRequestHandler> logger)
    : IRequestHandler<UpdateUserRequest>
{
    public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Trying to update user with id: {request.userId}");

        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            logger.LogError($"User with id: {request.userId} not found");

            throw new NotFoundException("No such user");
        }

        mapper.Map(request.userDTO, user);

        if (request.userDTO.Avatar is not null)
        {
            if (user.AvatarUrl is not null) 
            {
                await blobService.DeleteAsync(user.AvatarUrl);
            }

            using Stream stream = request.userDTO.Avatar.OpenReadStream();

            user.AvatarUrl = await blobService.UploadAsync(stream, request.userDTO.Avatar.ContentType);
        }

        await userManager.UpdateAsync(user);

        logger.LogInformation($"User with id: {request.userId} successfully updated");
    }
}
