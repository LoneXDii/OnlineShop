using UserService.Domain.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Authentication;

internal interface ITokenService
{
	Task<TokensDTO> GetTokensAsync(AppUser user);
	Task<string> RefreshAccessTokenAsync(string refreshToken);
}
