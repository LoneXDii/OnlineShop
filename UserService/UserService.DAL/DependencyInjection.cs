using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using System.Text;
using UserService.DAL.Entities;
using UserService.DAL.Database;
using UserService.DAL.Services.Authentication;
using UserService.DAL.Services.BlobStorage;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;

namespace UserService.DAL;

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

        services.AddStackExchangeRedisCache(opt =>
        {
            opt.Configuration = configuration["Redis:Configuration"];
            opt.InstanceName = configuration["Redis:InstanceName"];
        });

        services.AddScoped<IBlobService, BlobService>()
            .AddScoped(_ => new BlobServiceClient(configuration["ConnectionStrings:AzureConnection"]));

        services.AddScoped<ITokenService, TokenService>()
            .AddScoped<ISendGridClient>(sp => new SendGridClient(configuration["EmailAccount:ApiKey"]))
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<ICacheService, CacheService>();

        return services;
    }
}