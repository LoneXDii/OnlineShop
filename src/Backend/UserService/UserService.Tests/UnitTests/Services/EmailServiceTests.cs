using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using UserService.DAL.Models;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.EmailNotifications.MessageFactory;
using UserService.DAL.Services.EmailNotifications.MessageFactory.Models;

namespace UserService.Tests.UnitTests.Services;

public class EmailServiceTests
{
    private readonly Mock<ISendGridClient> _sendGridClientMock = new();
    private readonly Mock<IMessageFactory> _messageFactoryMock = new();
    private readonly Fixture _fixture = new();
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        var loggerMock = new Mock<ILogger<EmailService>>();
        
        _emailService = new EmailService(_sendGridClientMock.Object, loggerMock.Object, _messageFactoryMock.Object);
    }

    [Fact]
    public async Task SendEmailAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var email = "email";
        var code = "code";

        _messageFactoryMock.Setup(factory => factory.CreateEmailConfirmationMessage(code))
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendEmailConfirmationCodeAsync(email, code);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreateEmailConfirmationMessage(code),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendEmailConfirmationSucceededNotificationAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var email = "email";

        _messageFactoryMock.Setup(factory => factory.CreateEmailConfirmationSucceedMessage())
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendEmailConfirmationSucceededNotificationAsync(email);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreateEmailConfirmationSucceedMessage(),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendPasswordResetCodeAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var email = "email";
        var code = "code";

        _messageFactoryMock.Setup(factory => factory.CreatePasswordResetMessage(code))
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendPasswordResetCodeAsync(email, code);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreatePasswordResetMessage(code),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendPasswordResetSucceededNotificationAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var email = "email";

        _messageFactoryMock.Setup(factory => factory.CreatePasswordResetSucceedMessage())
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendPasswordResetSucceededNotificationAsync(email);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreatePasswordResetSucceedMessage(),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendUnconfirmedAccountDeletedNotificationAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var email = "email";

        _messageFactoryMock.Setup(factory => factory.CreateEmailConfirmationFailedMessage())
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendUnconfirmedAccountDeletedNotificationAsync(email);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreateEmailConfirmationFailedMessage(),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task SendEmailNotChangedNotificationAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var oldEmail = "oldEmail";
        var newEmail = "newEmail";

        _messageFactoryMock.Setup(factory => factory.CreateEmailChangeFailedMessage(oldEmail))
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendEmailNotChangedNotificationAsync(oldEmail, newEmail);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreateEmailChangeFailedMessage(oldEmail),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Exactly(2));
    }
    
    [Fact]
    public async Task SendOrderStatusNotificationAsync_WhenCalled_ShouldCreateMessageAndSendEmail()
    {
        //Arrange
        var order = _fixture.Create<ConsumedOrder>();

        _messageFactoryMock.Setup(factory => factory.CreateOrderStatusChangedMessage(order))
            .Returns(_fixture.Create<MessageModel>());
        
        //Act
        await _emailService.SendOrderStatusNotificationAsync(order);
        
        //Assert
        _messageFactoryMock.Verify(factory => factory.CreateOrderStatusChangedMessage(order),
            Times.Once);
        _sendGridClientMock.Verify(client => client.SendEmailAsync(It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}