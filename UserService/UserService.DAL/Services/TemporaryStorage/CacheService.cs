
using Microsoft.Extensions.Caching.Distributed;

namespace UserService.DAL.Services.TemporaryStorage;

internal class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<string?> GetEmailByCodeAsync(string code)
    {
        var email = await _cache.GetStringAsync($"email{code}");

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

        return code;
    }

    public async Task<string?> GetEmailByResetPasswordCodeAsync(string code)
    {
        var email = await _cache.GetStringAsync($"password{code}");

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

        return code;
    }
}
