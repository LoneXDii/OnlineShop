﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
<<<<<<< HEAD
using Microsoft.Extensions.Options;
=======
>>>>>>> feature/add-redis
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using System.Text.Json;

namespace OrderService.Infrastructure.Services;

internal class RedisStorageService : ITemporaryStorageService
{
    private readonly IDistributedCache _cache;
    private readonly HttpContext _httpContext;

    public RedisStorageService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
    {
        _cache = cache;
        _httpContext = httpContextAccessor.HttpContext;
    }
 
    public async Task<Dictionary<int, ProductEntity>> GetCartAsync(CancellationToken cancellationToken = default)
    {
<<<<<<< HEAD
        var cartId = _httpContext.User.FindFirst("Id")?.Value ?? _httpContext.Request.Cookies["CartId"];

        if(cartId is null)
        {
            cartId = Guid.NewGuid().ToString();
            _httpContext.Response.Cookies.Append("CartId", cartId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30), 
                HttpOnly = true, 
                Secure = true, 
            });
        }

=======
>>>>>>> feature/add-redis
        var userId = _httpContext.User.FindFirst("Id")?.Value;

        if (userId is not null)
        {
            var oldCartId = _httpContext.Request.Cookies["CartId"];

            if (oldCartId is not null)
            {
                var oldCart = await _cache.GetStringAsync(oldCartId, cancellationToken);

                _httpContext.Response.Cookies.Delete("CartId");

                if (oldCart is not null)
                {
                    await _cache.RemoveAsync(oldCartId, cancellationToken);

                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    };

                    await _cache.SetStringAsync(userId, oldCart, options, cancellationToken);
                }
            }
        }

<<<<<<< HEAD
        var cartJson = await _cache.GetStringAsync(cartId, cancellationToken);
=======
        var cartJson = await _cache.GetStringAsync(GetCartId(userId), cancellationToken);
>>>>>>> feature/add-redis

        var cart = cartJson is null 
            ? new Dictionary<int, ProductEntity>()
            : JsonSerializer.Deserialize<Dictionary<int, ProductEntity>>(cartJson);

        return cart;
    }

    public async Task SaveCartAsync(Dictionary<int, ProductEntity> cart, CancellationToken cancellationToken = default)
    {
<<<<<<< HEAD
        var cartId = _httpContext.User.FindFirst("Id")?.Value ?? _httpContext.Request.Cookies["CartId"];
=======
        var userId = _httpContext.User.FindFirst("Id")?.Value;

        var cartJson = JsonSerializer.Serialize(cart);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        await _cache.SetStringAsync(GetCartId(userId), cartJson, options, cancellationToken);
    }

    private string GetCartId(string? userId)
    {
        var cartId = userId ?? _httpContext.Request.Cookies["CartId"];
>>>>>>> feature/add-redis

        if (cartId is null)
        {
            cartId = Guid.NewGuid().ToString();
            _httpContext.Response.Cookies.Append("CartId", cartId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true,
            });
        }

<<<<<<< HEAD
        var cartJson = JsonSerializer.Serialize(cart);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        if (_httpContext.User.FindFirst("Id")?.Value is not null)
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
        }

        await _cache.SetStringAsync(cartId, cartJson, options, cancellationToken);
=======
        return cartId;
>>>>>>> feature/add-redis
    }
}
