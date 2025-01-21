using System.Linq.Expressions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UserService.BLL.Exceptions;
using UserService.BLL.Proxy;
using UserService.BLL.UseCases.AuthUseCases.ResendEmailConfirmationCodeUseCase;
using UserService.DAL.Entities;
using UserService.DAL.Services.EmailNotifications;
using UserService.DAL.Services.TemporaryStorage;
using UserService.Tests.Factories;
using UserService.Tests.Setups;

namespace UserService.Tests.UnitTests.UseCases.AuthUseCases;

public class ResendEmailConfirmationRequestHandlerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<IEmailService> _emailServiceMock = new();
        private readonly Mock<ICacheService> _cacheMock = new();
        private readonly Mock<ILogger<ResendEmailConfirmationRequestHandler>> _loggerMock = new();
        private readonly Mock<IBackgroundJobProxy> _backgroundJobProxyMock = new();
        private readonly ResendEmailConfirmationRequestHandler _handler;
        private readonly Fixture _fixture = new();

        public ResendEmailConfirmationRequestHandlerTests()
        {
            _userManagerMock = MocksFactory.CreateUserManager();
            
            _backgroundJobProxyMock.SetupBackgroundJobProxy();
            
            _handler = new ResendEmailConfirmationRequestHandler(
                _userManagerMock.Object,
                _emailServiceMock.Object,
                _cacheMock.Object,
                _loggerMock.Object,
                _backgroundJobProxyMock.Object);
        }

        [Fact]
        public async Task Handle_WhenUserDoesNotExist_ShouldThrowBadRequestException()
        {
            //Arrange
            var request = _fixture.Create<ResendEmailConfirmationCodeRequest>();
            _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync((AppUser?)null);

            //Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal("No such user", exception.Message);
            _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
            _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenEmailAlreadyConfirmed_ShouldThrowBadRequestException()
        {
            //Arrange
            var request = _fixture.Create<ResendEmailConfirmationCodeRequest>();
            var user = _fixture.Create<AppUser>();
            user.EmailConfirmed = true;

            _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            //Act
            var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal("Email already confirmed", exception.Message);
            _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Never);
            _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenUserExistsAndEmailNotConfirmed_ShouldSendConfirmationCode()
        {
            //Arrange
            var request = _fixture.Create<ResendEmailConfirmationCodeRequest>();
            var user = _fixture.Create<AppUser>();
            user.EmailConfirmed = false;

            _userManagerMock.Setup(manager => manager.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);
            _cacheMock.Setup(cache => cache.SetEmailConfirmationCodeAsync(request.Email))
                .ReturnsAsync("confirmationCode");

            //Act
            await _handler.Handle(request, CancellationToken.None);

            //Assert
            _cacheMock.Verify(cache => cache.SetEmailConfirmationCodeAsync(request.Email), Times.Once);
            _backgroundJobProxyMock.Verify(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()), Times.Once);
            _emailServiceMock.Verify(service => service.SendEmailConfirmationCodeAsync(request.Email, "confirmationCode"), Times.Once);
        }
    }