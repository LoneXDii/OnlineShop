using Microsoft.Extensions.Options;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using Stripe;
using Stripe.Checkout;

namespace OrderService.Infrastructure.Services;

internal class PaymentService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;
    private readonly CustomerService _customerService;
    private readonly ProductService _productService;
    private readonly PriceService _priceService;

    public PaymentService(IOptions<StripeSettings> stripeSettings,
        CustomerService customerService,
        ProductService productService,
        PriceService priceService)
    {
        _stripeSettings = stripeSettings.Value;
        _customerService = customerService;
        _productService = productService;
        _priceService = priceService;

        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<string> PayAsync(OrderEntity order, string customerId)
    {
        var items = new List<SessionLineItemOptions>();

        foreach (var product in order.Products)
        {
            items.Add(new SessionLineItemOptions
            {
                Price = product.PriceId,
                Quantity = product.Quantity
            });
        }

        var options = new SessionCreateOptions
        {
            LineItems = items,
            Mode = _stripeSettings.SessionMode,
            SuccessUrl = _stripeSettings.SuccessUrl,
            CancelUrl = _stripeSettings.CancelUrl,
            Customer = customerId,
            Metadata = new Dictionary<string, string>
            {
                { "order_id", order.Id }
             }
        };

        var sessionService = new SessionService();

        var session = await sessionService.CreateAsync(options);

        return session.Url;
    }

    public async Task<string> CreateCustomerAsync(string email, string name)
    {
        var customerOptions = new CustomerCreateOptions
        {
            Email = email
        };

        var customer = await _customerService.CreateAsync(customerOptions);

        return customer.Id;
    }

    public async Task<string> CreateProductAsync(ProductEntity product)
    {
        var productOptions = new ProductCreateOptions
        {
            Name = product.Name
        };

        var stripeProduct = await _productService.CreateAsync(productOptions);

        var priceOptions = new PriceCreateOptions
        {
            Product = stripeProduct.Id,
            UnitAmount = (long)(product.Price * 100),
            Currency = "usd"
        };

        var price = await _priceService.CreateAsync(priceOptions);

        return price.Id;
    }

    public string? GetSuccessPaymentOrderId(string eventJson, string signature)
    {
        var stripeEvent = EventUtility.ConstructEvent(eventJson, signature, _stripeSettings.Secret);
        
        if(stripeEvent.Type is not EventTypes.CheckoutSessionCompleted)
        {
            return null;
        }

        var session = stripeEvent.Data.Object as Session;

        return session?.Metadata["order_id"];
    }
}
