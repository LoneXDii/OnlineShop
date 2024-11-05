using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Domain.Entities;
using UserService.Infrastructure.Services.BlobStorage;

namespace UserService.Application.UseCases.AuthUseCases.RegisterUserUseCase;

internal class RegisterUserRequestHandler(UserManager<AppUser> userManager, IMapper mapper, IBlobService blobService)
	: IRequestHandler<RegisterUserRequest>
{
	public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
	{
		var user = mapper.Map<AppUser>(request.RegisterModel);

		if(request.RegisterModel.Avatar is not null)
		{
			using Stream stream = request.RegisterModel.Avatar.OpenReadStream();
			var imageId = await blobService.UploadAsync(stream, request.RegisterModel.Avatar.ContentType);

			//Add image url(later replace by Ocelot endpoint)
			user.AvatarUrl = imageId.ToString();
		}

		await userManager.CreateAsync(user, request.RegisterModel.Password);
	}
}
