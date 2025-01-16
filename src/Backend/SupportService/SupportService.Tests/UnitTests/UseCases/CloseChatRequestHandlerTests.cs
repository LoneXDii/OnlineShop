using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Application.Proxy;
using SupportService.Application.UseCases.CloseChat;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;
using System.Linq.Expressions;

namespace SupportService.Tests.UnitTests.UseCases;

public class CloseChatRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<CloseChatRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobProxyMock = new();
    private readonly CloseChatRequestHandler _handler;

    public CloseChatRequestHandlerTests()
    {
        _handler = new CloseChatRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _backgroundJobProxyMock.Object);
    }

    [Fact]
    public async Task Handle_WhenChatIsNotExists_ShouldThrowBadRequestException()
    {
        // Arrange
        var request = new CloseChatRequest(1, "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Chat?)null);

        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        // Assert
        Assert.Equal("No such chat", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.UpdateAsync(It.IsAny<Chat>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);

        _backgroundJobProxyMock.Verify(backroundJob =>
            backroundJob.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoAccessToChat_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new CloseChatRequest(1, "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat { ClientId = "2" });

        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        // Assert
        Assert.Equal("You have no access to this chat", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.UpdateAsync(It.IsAny<Chat>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);

        _backgroundJobProxyMock.Verify(backroundJob =>
            backroundJob.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WherUserIsChatOwner_ShouldReturnClosedChat()
    {
        //Arrange
        var request = new CloseChatRequest(1, "1");
        var chat = new Chat { Id = 1, ClientId = "1", IsActive = true };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        _mapperMock.Setup(mapper => mapper.Map<ChatDTO>(It.IsAny<Chat>()))
            .Returns((Chat chat) => new ChatDTO
            {
                Id = chat.Id,
                IsActive = chat.IsActive
            });

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
        Assert.False(result.IsActive);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.UpdateAsync(chat, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        _backgroundJobProxyMock.Verify(backroundJob =>
            backroundJob.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WherRequestedByAdmin_ShouldReturnClosedChat()
    {
        //Arrange
        var request = new CloseChatRequest(1, null);
        var chat = new Chat { Id = 1, ClientId = "1", IsActive = true };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        _mapperMock.Setup(mapper => mapper.Map<ChatDTO>(It.IsAny<Chat>()))
            .Returns((Chat chat) => new ChatDTO
            {
                Id = chat.Id,
                IsActive = chat.IsActive
            });

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
        Assert.False(result.IsActive);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.UpdateAsync(chat, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        _backgroundJobProxyMock.Verify(backroundJob =>
            backroundJob.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<TimeSpan>()), Times.Once);
    }
}
