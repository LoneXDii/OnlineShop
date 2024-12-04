namespace UserService.DAL.Services.TemporaryStorage;

public interface ICacheService
{
    Task<string?> GetEmailByCodeAsync(string code);
    Task<string?> SetEmailConfirmationCodeAsync(string email);
}
