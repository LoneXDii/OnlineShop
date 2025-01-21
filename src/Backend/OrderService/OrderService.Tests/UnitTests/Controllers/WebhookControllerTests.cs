using System.Text;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using OrderService.API.Controllers;
using OrderService.Application.UseCases.PaymentUseCases.ConfirmPaymentUseCase;

namespace OrderService.Tests.UnitTests.Controllers;

public class WebhookControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly WebhookController _controller;

    public WebhookControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new WebhookController(_mediatorMock.Object);
    }

    [Fact]
    public async Task StripeWebhookHandler_WhenCalled_ShouldCallConfirmPaymentRequest()
    {
        //Arrange
        var json = "{\"test\":\"data\"}";
        var signature = "test-signature";
        var context = new DefaultHttpContext();
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(json));
        context.Request.Headers["Stripe-Signature"] = signature;
        _controller.ControllerContext.HttpContext = context;

        //Act
        await _controller.StripeWebhookHandler(CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(It.IsAny<ConfirmPaymentRequest>(), CancellationToken.None), Times.Once);
    }
}