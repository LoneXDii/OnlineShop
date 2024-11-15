using Microsoft.AspNetCore.Identity;

namespace UserService.DAL.Entities;

public class AppUser : IdentityUser
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string? AvatarUrl { get; set; }
	public string? RefreshTokenValue { get; set; }
	public DateTime? RefreshTokenExpiresAt { get; set; }
}
