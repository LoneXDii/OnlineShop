namespace OrderService.Infrastructure.Configuration;

internal class StripeSettings
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
    public string Secret { get; set; }
    public string SessionMode { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}
