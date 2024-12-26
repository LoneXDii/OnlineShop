
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace UserService.DAL.Services.TemporaryStorage;

internal class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<CacheService> _logger;

    public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<string?> GetEmailByCodeAsync(string code)
    {
        var email = await _cache.GetStringAsync($"email{code}");

        _logger.LogInformation($"Got email: {email} from cache");

        return email;
    }

    public async Task<string> SetEmailConfirmationCodeAsync(string email)
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();

        await _cache.SetStringAsync($"email{code}", email, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        _logger.LogInformation($"Set email: {email} to cache");

        return code;
    }

    public async Task<string?> GetEmailByResetPasswordCodeAsync(string code)
    {
        var email = await _cache.GetStringAsync($"password{code}");

        _logger.LogInformation($"Got email: {email} from cache by password reset code");

        return email;
    }

    public async Task<string> SetResetPasswordCodeAsync(string email)
    {
        var random = new Random();
        var code = random.Next(100000, 999999).ToString();

        await _cache.SetStringAsync($"password{code}", email, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        _logger.LogInformation($"Set password reset code for email: {email}");

        return code;
    }
}
