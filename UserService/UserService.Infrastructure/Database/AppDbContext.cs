using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Database;

internal class AppDbContext : IdentityDbContext<AppUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<AppUser> AppUsers { get; set; }
}
