using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.BlobStorage;

namespace UserService.BLL.UseCases.UserUseCases.UpdateUserUseCase;

internal class UpdateUserRequestHandler(UserManager<AppUser> userManager, IMapper mapper, IBlobService blobService)
	: IRequestHandler<UpdateUserRequest>
{
	public async Task Handle(UpdateUserRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByIdAsync(request.userId);

		if (user is null)
		{
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
	}
}
