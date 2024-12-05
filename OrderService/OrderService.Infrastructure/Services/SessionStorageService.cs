using Microsoft.AspNetCore.Http;
using OrderService.Domain.Abstractions.Data;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Services;

internal class SessionStorageService : ITemporaryStorageService
{
    public ISession Session { get; set; }

    public SessionStorageService(IHttpContextAccessor httpContextAccessor)
    {
        Session = httpContextAccessor.HttpContext.Session;
    }

    public void SaveCart(Dictionary<int, ProductEntity> cart)
    {
        Session.Set<Dictionary<int, ProductEntity>>("cart", cart);
    }

    public Dictionary<int, ProductEntity> GetCart()
    {
        var cart = Session.Get<Dictionary<int, ProductEntity>>("cart") ?? new Dictionary<int, ProductEntity>();

        return cart;
    }
}
