﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.DAL.Entities;
using UserService.BLL.Exceptions;
using UserService.DAL.Services.EmailNotifications;
using System.Text.Json;

namespace UserService.BLL.UseCases.AuthUseCases.EmailConfirmationUseCase;

internal class EmailConfirmationRequestHandler(UserManager<AppUser> userManager, IEmailService emailService)
    : IRequestHandler<EmailConfirmationRequest>
{
    public async Task Handle(EmailConfirmationRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.email);

        if (user is null)
        {
            throw new NotFoundException("No user with such email");
        }

        var result = await userManager.ConfirmEmailAsync(user, request.code);

        if (!result.Succeeded)
        {
            var errors = JsonSerializer.Serialize(result.Errors);

            throw new BadRequestException($"Email is not confirmed: {errors}");
        }

        await emailService.SendEmailConfirmationSucceededNotificationAsync(user.Email);
    }
}
