using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Domain.Entities;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Authentication;

internal class TokenService : ITokenService
{
	private readonly IConfiguration _configuration;
	private readonly UserManager<AppUser> _userManager;

	public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
	{
		_configuration = configuration;
		_userManager = userManager;
	}

	public async Task<TokensDTO> GetTokens(AppUser user)
	{
		var tokens = new TokensDTO();

		tokens.AccessToken = await GetAccessToken(user);
		tokens.RefreshToken = await GetRefreshToken(user);

		return tokens;
	}

	public async Task<string> RefreshAccessToken(string refreshToken)
	{
		var user = _userManager.Users.FirstOrDefault(u => u.RefreshTokenValue == refreshToken);
		if (user is null)
		{
			throw new NotImplementedException();
		}
		var token = await GetAccessToken(user);
		return token;
	}

	private async Task<string> GetAccessToken(AppUser user)
	{
		var claims = new List<Claim>
		{
			 new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			 new(ClaimTypes.Email, user.Email),
			 new("Avatar", user.AvatarUrl ?? "")
		};

		var roles = await _userManager.GetRolesAsync(user);
		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
		var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JWT:AccessTokenExpiryMinutes"]));

		var token = new JwtSecurityToken
			(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: expires,
				claims: claims,
				signingCredentials: credentials
			);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	private async Task<string> GetRefreshToken(AppUser user)
	{
		user.RefreshTokenValue = Guid.NewGuid().ToString();
		user.RefreshTokenExpiresAt = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:RefreshExpireDays"]));
		await _userManager.UpdateAsync(user);
		
		return user.RefreshTokenValue;
	}
}
