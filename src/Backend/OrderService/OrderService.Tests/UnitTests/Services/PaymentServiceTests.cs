using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using OrderService.Domain.Abstractions.Payments;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Configuration;
using OrderService.Infrastructure.Proxy;
using OrderService.Infrastructure.Services;
using Stripe;
using Stripe.Checkout;

namespace OrderService.Tests.UnitTests.Services;

public class PaymentServiceTests
{
    private readonly Mock<CustomerService> _customerServiceMock = new();
    private readonly Mock<ProductService> _productServiceMock = new();
    private readonly Mock<PriceService> _priceServiceMock = new();
    private readonly Mock<CouponService> _couponServiceMock = new();
    private readonly Mock<SessionService> _sessionServiceMock = new();
    private readonly Mock<ILogger<PaymentService>> _loggerMock = new();
    private readonly Mock<IStripeEventUtilityProxy> _stripeEventUtilityProxyMock = new();
    private readonly Fixture _fixture = new();
    private readonly StripeSettings _stripeSettings;
    private readonly PaymentService _paymentService;
    
    public PaymentServiceTests()
    {
        _stripeSettings = _fixture.Create<StripeSettings>();
        var options = Options.Create(_stripeSettings);
        
        _paymentService = new PaymentService(options,
            _loggerMock.Object,
            _customerServiceMock.Object,
            _productServiceMock.Object,
            _priceServiceMock.Object,
            _couponServiceMock.Object,
            _sessionServiceMock.Object,
            _stripeEventUtilityProxyMock.Object);
    }

    [Fact]
    public async Task CreateCustomerAsync_WhenCalled_ShouldCreateCustomerAndReturnId()
    {
        //Arrange
        var email = "email";
        var name = "name";
        
        _customerServiceMock.Setup(customerService => customerService.CreateAsync(It.IsAny<CustomerCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Customer{Id = "Id"});
        
        //Act
        var result = await _paymentService.CreateCustomerAsync(email, name);
        
        //Assert
        Assert.Equal("Id", result);
        
        _customerServiceMock.Verify(customerService => customerService.CreateAsync(It.IsAny<CustomerCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateProductAsync_WhenCalled_ShouldCreateProductAndReturnPriceId()
    {
        //Arrange
        var name = "name";
        var price = 123;
        
        _productServiceMock.Setup(productService => productService.CreateAsync(It.IsAny<ProductCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product{Id = "ProductId"});
        
        _priceServiceMock.Setup(priceService => priceService.CreateAsync(It.IsAny<PriceCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Price{Id = "PriceId"});
        
        //Act
        var result = await _paymentService.CreateProductAsync(name, price);
        
        //Assert
        Assert.Equal("PriceId", result);
        
        _productServiceMock.Verify(productService => productService.CreateAsync(It.IsAny<ProductCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()), 
            Times.Once);
        
        _priceServiceMock.Verify(priceService => priceService.CreateAsync(It.IsAny<PriceCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task PayAsync_WhenNoDiscounts_ShouldCreatePaymentSessionWithoutCreatingDiscountAndReturnSessionUrl()
    {
        //Arrange
        var order = _fixture.Create<OrderEntity>();

        foreach (var product in order.Products)
        {
            product.Discount = 0;
        }
        
        _sessionServiceMock.Setup(sessionService => sessionService.CreateAsync(It.IsAny<SessionCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session{Url = "Url"});
        
        //Act
        var result = await _paymentService.PayAsync(order, "id");
        
        //Assert
        Assert.Equal("Url", result);
        
        _sessionServiceMock.Verify(sessionService => sessionService.CreateAsync(It.IsAny<SessionCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()),
            Times.Once);
        
        _couponServiceMock.Verify(couponService => couponService.CreateAsync(It.IsAny<CouponCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task PayAsync_WhenThereAreDiscount_ShouldCreatePaymentSessionWithCreatingDiscountAndReturnSessionUrl()
    {
        //Arrange
        var order = _fixture.Create<OrderEntity>();

        foreach (var product in order.Products)
        {
            product.Discount = 1;
        }
        
        _sessionServiceMock.Setup(sessionService => sessionService.CreateAsync(It.IsAny<SessionCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session{Url = "Url"});
        
        _couponServiceMock.Setup(couponService => couponService.CreateAsync(It.IsAny<CouponCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Coupon{Id = "CouponId"});
        
        //Act
        var result = await _paymentService.PayAsync(order, "id");
        
        //Assert
        Assert.Equal("Url", result);
        
        _sessionServiceMock.Verify(sessionService => sessionService.CreateAsync(It.IsAny<SessionCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()),
            Times.Once);
        
        _couponServiceMock.Verify(couponService => couponService.CreateAsync(It.IsAny<CouponCreateOptions>(), It.IsAny<RequestOptions>(),It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public void GetSuccessPaymentOrderId_WhenAllIsValid_ShouldReturnOrderId()
    {
        //Arrange
        var eventJson = "event";
        var signature = "signature";
        
        var session = new Session
        {
            Id = "sessionId",
            Metadata = new Dictionary<string, string>
            {
                { "order_id", "orderId" }
            }
        };
        
        var stripeEvent = new Event
        {
            Type = EventTypes.CheckoutSessionCompleted,
            Data = new EventData
            {
                Object = session
            }
        };
        
        _stripeEventUtilityProxyMock.Setup(proxy => proxy.ConstructEvent(eventJson, signature, _stripeSettings.Secret))
            .Returns(stripeEvent);
        
        //Act
        var orderId = _paymentService.GetSuccessPaymentOrderId(eventJson, signature);
        
        //Assert
        Assert.NotNull(orderId);
        Assert.Equal("orderId", orderId);
        _stripeEventUtilityProxyMock.Verify(proxy => proxy.ConstructEvent(eventJson, signature, _stripeSettings.Secret),
            Times.Once);
    }
    
    [Fact]
    public void GetSuccessPaymentOrderId_WhenWrongEventType_ShouldReturnNull()
    {
        //Arrange
        var eventJson = "event";
        var signature = "signature";
        
        var stripeEvent = new Event
        {
            Type = "Wrong Type",
        };
        
        _stripeEventUtilityProxyMock.Setup(proxy => proxy.ConstructEvent(eventJson, signature, _stripeSettings.Secret))
            .Returns(stripeEvent);
        
        //Act
        var orderId = _paymentService.GetSuccessPaymentOrderId(eventJson, signature);
        
        //Assert
        Assert.Null(orderId);
        _stripeEventUtilityProxyMock.Verify(proxy => proxy.ConstructEvent(eventJson, signature, _stripeSettings.Secret),
            Times.Once);
    }
}