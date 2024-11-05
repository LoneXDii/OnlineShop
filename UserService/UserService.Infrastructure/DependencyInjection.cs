using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Entities;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Services.BlobStorage;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AppDbContext>(opt =>
						opt.UseMySql(configuration["ConnectionStrings:MySQLConnection"],
						new MySqlServerVersion(new Version(8, 0, 36)),
						opt => opt.EnableRetryOnFailure()),
					ServiceLifetime.Scoped);

		services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

		services.AddScoped<IBlobService, BlobService>()
				.AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

		return services;
	}
}