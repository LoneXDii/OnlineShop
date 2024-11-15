using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using System.Web;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.BlobStorage;
using UserService.DAL.Services.EmailNotifications;

namespace UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;

internal class RegisterUserRequestHandler(UserManager<AppUser> userManager, IMapper mapper, IBlobService blobService, 
	IEmailService emailService) : IRequestHandler<RegisterUserRequest>
{
	public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
	{
		var user = mapper.Map<AppUser>(request.RegisterModel);

		if(request.RegisterModel.Avatar is not null)
		{
			using Stream stream = request.RegisterModel.Avatar.OpenReadStream();
			var imageId = await blobService.UploadAsync(stream, request.RegisterModel.Avatar.ContentType);

			//Later will be replace by Ocelot endpoint
			var imageUrl = $"http://127.0.0.1:10000/devstoreaccount1/avatars/{imageId}";
			user.AvatarUrl = imageUrl;
		}

		var result = await userManager.CreateAsync(user, request.RegisterModel.Password);
		if (result.Succeeded)
		{
			var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
			code = HttpUtility.UrlEncode(code);
			var email = HttpUtility.UrlEncode(user.Email);
			var confirmationLink = $"https://localhost:7001/api/account/confirm/email={email}&code={code}";
			await emailService.SendEmailConfirmationCodeAsync(user.Email, confirmationLink);
		}
		else
		{
			var errors = JsonSerializer.Serialize(result.Errors);
			throw new RegisterException($"Cannot register user: {errors}");
		}
	}
}
