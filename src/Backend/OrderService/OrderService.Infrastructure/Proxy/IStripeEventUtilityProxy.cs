using Stripe;

namespace OrderService.Infrastructure.Proxy;

public interface IStripeEventUtilityProxy
{
    public Event ConstructEvent(string json, string stripeSignatureHeader, string secret);
}