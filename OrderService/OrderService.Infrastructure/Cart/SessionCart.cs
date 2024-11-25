using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Abstractions.Cart;
using OrderService.Domain.Entities;
using System.Text.Json.Serialization;
using OrderService.Infrastructure.Extensions;

namespace OrderService.Infrastructure.Cart;

internal class SessionCart : AbstractCart
{
    [JsonIgnore]
    public ISession? Session { get; set; }

    public static ICart GetCart(IServiceProvider services)
    {
        ISession session = services.GetRequiredService<IHttpContextAccessor>().HttpContext.Session;
        SessionCart cart = session.Get<SessionCart>("cart") ?? new SessionCart();
        cart.Session = session;

        return cart;
    }

    public override void AddToCart(ProductEntity product)
    {
        base.AddToCart(product);
        Session?.Set<SessionCart>("cart", this);
    }
    public override void ReduceInCart(int id, int quantity)
    {
        base.ReduceInCart(id, quantity);
        Session?.Set<SessionCart>("cart", this);
    }

    public override void RemoveItems(int id)
    {
        base.RemoveItems(id);
        Session?.Set<SessionCart>("cart", this);
    }

    public override void ClearAll()
    {
        base.ClearAll();
        Session?.Remove("cart");
    }
}
