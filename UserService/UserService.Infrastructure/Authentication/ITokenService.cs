using UserService.Domain.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Authentication;

internal interface ITokenService
{
	Task<TokensDTO> GetTokens(AppUser user);
	Task<string> RefreshAccessToken(string refreshToken);
}
