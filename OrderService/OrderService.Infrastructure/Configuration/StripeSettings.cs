namespace OrderService.Infrastructure.Configuration;

internal class StripeSettings
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
    public string Secret { get; set; }
}
