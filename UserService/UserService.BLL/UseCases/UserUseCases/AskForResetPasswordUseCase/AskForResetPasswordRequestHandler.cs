using MediatR;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.BLL.UseCases.UserUseCases.AskForResetPasswordUseCase;

internal class AskForResetPasswordRequestHandler(IEmailService emailService, ICacheService cacheService)
    : IRequestHandler<AskForResetPasswordRequest>
{
    public async Task Handle(AskForResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var code = await cacheService.SetResetPasswordCodeAsync(request.Email);

        await emailService.SendPasswordResetCodeAsync(request.Email, code);
    }
}
