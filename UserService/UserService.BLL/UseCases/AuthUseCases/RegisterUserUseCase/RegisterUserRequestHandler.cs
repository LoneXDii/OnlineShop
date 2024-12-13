using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.BlobStorage;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using Hangfire;

namespace UserService.BLL.UseCases.AuthUseCases.RegisterUserUseCase;

internal class RegisterUserRequestHandler(UserManager<AppUser> userManager, IMapper mapper, IBlobService blobService, 
    IEmailService emailService, ICacheService cacheService) : IRequestHandler<RegisterUserRequest>
{
    public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<AppUser>(request.RegisterModel);

        if(request.RegisterModel.Avatar is not null)
        {
            using Stream stream = request.RegisterModel.Avatar.OpenReadStream();

            user.AvatarUrl = await blobService.UploadAsync(stream, request.RegisterModel.Avatar.ContentType);
        }

        var result = await userManager.CreateAsync(user, request.RegisterModel.Password);

        if (result.Succeeded)
        {
            var code = await cacheService.SetEmailConfirmationCodeAsync(user.Email);

            BackgroundJob.Enqueue(() => emailService.SendEmailConfirmationCodeAsync(user.Email, code));

            BackgroundJob.Schedule(() => DeleteUnconfirmedUser(user.Email), TimeSpan.FromMinutes(15));
        }
        else
        {
            var errors = JsonSerializer.Serialize(result.Errors);

            throw new BadRequestException($"Cannot register user: {errors}");
        }
    }

    public async Task DeleteUnconfirmedUser(string email)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (!user.EmailConfirmed)
        {
            if (user.AvatarUrl is not null)
            {
                await blobService.DeleteAsync(user.AvatarUrl);
            }

            await userManager.DeleteAsync(user);
        }
    }
}
