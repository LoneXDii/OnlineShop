using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.DTO;
using UserService.Infrastructure.Entities;
using UserService.Application.Exceptions;

namespace UserService.Application.UseCases.UserUseCases.GetUserInfoUseCase;

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
