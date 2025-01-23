using Stripe;

namespace OrderService.Infrastructure.Proxy;

public class StripeEventUtilityProxy : IStripeEventUtilityProxy
{
    public Event ConstructEvent(string json, string stripeSignatureHeader, string secret)
    {
        return EventUtility.ConstructEvent(json, stripeSignatureHeader, secret);
    }
}