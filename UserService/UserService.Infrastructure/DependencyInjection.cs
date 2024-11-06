using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using System.Text;
using UserService.Domain.Entities;
using UserService.Infrastructure.Database;
using UserService.Infrastructure.Database.DbInitializer;
using UserService.Infrastructure.Services.Authentication;
using UserService.Infrastructure.Services.BlobStorage;
using UserService.Infrastructure.Services.EmailNotifications;

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

		services.AddAuthentication(options => 
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(opt =>
				opt.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["JWT:Issuer"],
					ValidAudience = configuration["JWT:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
				});

		services.AddScoped<IBlobService, BlobService>()
			.AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

		services.AddScoped<ITokenService, TokenService>()
			.AddScoped<ISendGridClient>(sp => new SendGridClient(configuration["EmailAccount:ApiKey"]))
			.AddScoped<IEmailService, EmailService>()
			.AddScoped<IDbInitializer, DbInitializer>();

		return services;
	}
}