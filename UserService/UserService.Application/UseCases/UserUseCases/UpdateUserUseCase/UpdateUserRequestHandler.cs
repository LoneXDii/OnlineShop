using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Infrastructure.Entities;
using UserService.Application.Exceptions;
using UserService.Infrastructure.Services.BlobStorage;

namespace UserService.Application.UseCases.UserUseCases.UpdateUserUseCase;

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
				var avatarId = user.AvatarUrl.Split("/").Last();
				await blobService.DeleteAsync(new Guid(avatarId));
			}

			using Stream stream = request.userDTO.Avatar.OpenReadStream();
			var imageId = await blobService.UploadAsync(stream, request.userDTO.Avatar.ContentType);

			//Later will be replace by Ocelot endpoint
			var imageUrl = $"http://127.0.0.1:10000/devstoreaccount1/avatars/{imageId}";
			user.AvatarUrl = imageUrl;
		}

		await userManager.UpdateAsync(user);
	}
}
