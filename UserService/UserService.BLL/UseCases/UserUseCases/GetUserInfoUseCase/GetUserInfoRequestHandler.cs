using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.BLL.DTO;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;

namespace UserService.BLL.UseCases.UserUseCases.GetUserInfoUseCase;

internal class GetUserInfoRequestHandler(UserManager<AppUser> userManager, IMapper mapper)
    : IRequestHandler<GetUserInfoRequest, UserInfoDTO>
{
    public async Task<UserInfoDTO> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.userId);

        if (user is null)
        {
            throw new NotFoundException("No such user");
        }

        var userDTO = mapper.Map<UserInfoDTO>(user);

        return userDTO;
    }
}
