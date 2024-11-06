
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Domain.Entities;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace UserService.Infrastructure.Database.DbInitializer;

internal class DbInitializer : IDbInitializer
{
	private readonly AppDbContext _dbContext;
	private readonly UserManager<AppUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public DbInitializer(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext dbContext)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_dbContext = dbContext;
	}

	public async Task SeedDataAsync()
	{
		await _dbContext.Database.MigrateAsync();

		if (await _roleManager.FindByNameAsync("admin") is null)
		{
			await _roleManager.CreateAsync(new IdentityRole("admin"));
		}
		else
		{
			return;
		}

		AppUser adminUser = new()
		{
			UserName = "admin1@gmail.com",
			Email = "admin1@gmail.com",
			EmailConfirmed = true,
			FirstName = "Admin",
			LastName = "Admin"
		};
		await _userManager.CreateAsync(adminUser, "Qwe123*");
		await _userManager.AddToRoleAsync(adminUser, "admin");

		AppUser customerUser = new()
		{
			UserName = "client1@gmail.com",
			Email = "client1@gmail.com",
			EmailConfirmed = true,
			FirstName = "Customer",
			LastName = "Customer"
		};
		await _userManager.CreateAsync(customerUser, "Qwe123*");
	}
}
