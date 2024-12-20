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
    private readonly CouponService _couponService;

    public PaymentService(IOptions<StripeSettings> stripeSettings,
        CustomerService customerService,
        ProductService productService,
        PriceService priceService,
        CouponService couponService)
    {
        _stripeSettings = stripeSettings.Value;
        _customerService = customerService;
        _productService = productService;
        _priceService = priceService;
        _couponService = couponService;

        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<string> PayAsync(OrderEntity order, string customerId)
    {
        var items = new List<SessionLineItemOptions>();
        double discount = 0;

        foreach (var product in order.Products)
        {
            items.Add(new SessionLineItemOptions
            {
                Price = product.PriceId,
                Quantity = product.Quantity,
            });

            if (product.Discount > 0)
            {
                discount += (product.Price * product.Quantity * product.Discount / 100);
            }
        }

        var couponOptions = new CouponCreateOptions
        {
            Duration = "once", 
            AmountOff = (long)(discount * 100), 
            Currency = "usd"
        };

        var coupon = await _couponService.CreateAsync(couponOptions);

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
            },
            Discounts = new List<SessionDiscountOptions>
            {
                new SessionDiscountOptions
                {
                    Coupon = coupon.Id
                }
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

    public async Task<string> CreateProductAsync(string name, double price)
    {
        var productOptions = new ProductCreateOptions
        {
            Name = name
        };

        var stripeProduct = await _productService.CreateAsync(productOptions);

        var priceOptions = new PriceCreateOptions
        {
            Product = stripeProduct.Id,
            UnitAmount = (long)(price * 100),
            Currency = "usd"
        };

        var stripePrice = await _priceService.CreateAsync(priceOptions);

        return stripePrice.Id;
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
