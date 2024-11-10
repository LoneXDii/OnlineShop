using UserService.Domain.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Services.Authentication;

public interface ITokenService
{
    Task<TokensDTO> GetTokensAsync(AppUser user);
    Task<string> RefreshAccessTokenAsync(string refreshToken);
    Task InvalidateRefreshTokenAsync(AppUser user);
}
