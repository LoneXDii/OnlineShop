using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;

internal class AskForResetPasswordRequestHandler(IEmailService emailService, ICacheService cacheService,
    ILogger<AskForResetPasswordRequestHandler> logger)
    : IRequestHandler<AskForResetPasswordRequest>
{
    public async Task Handle(AskForResetPasswordRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"User with email: {request.Email} asked for password reset");

        var code = await cacheService.SetResetPasswordCodeAsync(request.Email);

        BackgroundJob.Enqueue(() => emailService.SendPasswordResetCodeAsync(request.Email, code));
    }
}
