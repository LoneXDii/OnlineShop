using UserService.DAL.Entities;
using UserService.DAL.Models;

namespace UserService.DAL.Services.Authentication;

public interface ITokenService
{
    Task<TokensDTO> GetTokensAsync(AppUser user);
    Task<string?> RefreshAccessTokenAsync(string refreshToken);
    Task InvalidateRefreshTokenAsync(AppUser user);
    Task<string> GetRefreshTokenAsync(AppUser user);
}
