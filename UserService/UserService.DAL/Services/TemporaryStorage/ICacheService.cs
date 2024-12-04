namespace UserService.DAL.Services.TemporaryStorage;

public interface ICacheService
{
    Task<string?> GetEmailByCodeAsync(string code);
    Task<string> SetEmailConfirmationCodeAsync(string email);
    Task<string?> GetEmailByResetPasswordCodeAsync(string code);
    Task<string> SetResetPasswordCodeAsync(string email);
}
