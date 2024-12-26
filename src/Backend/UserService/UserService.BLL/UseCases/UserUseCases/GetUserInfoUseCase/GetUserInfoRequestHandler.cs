using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.BLL.DTO;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using Microsoft.Extensions.Logging;

namespace UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;

internal class GetUserInfoRequestHandler(UserManager<AppUser> userManager, IMapper mapper, 
    ILogger<GetUserInfoRequestHandler> logger)
    : IRequestHandler<GetUserInfoRequest, UserInfoDTO>
{
    public async Task<UserInfoDTO> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            logger.LogError($"No user with id: {request.userId}");

            throw new NotFoundException("No such user");
        }

        var userDTO = mapper.Map<UserInfoDTO>(user);

        return userDTO;
    }
}
