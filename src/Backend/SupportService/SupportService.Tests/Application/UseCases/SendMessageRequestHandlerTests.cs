using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Application.UseCases.SendMessage;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Tests.Application.UseCases;

public class SendMessageRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<SendMessageRequestHandler>> _loggerMock = new();
    private readonly SendMessageRequestHandler _handler;

    public SendMessageRequestHandlerTests()
    {
        _handler = new SendMessageRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenChatIsNotExists_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new SendMessageRequest(new(), "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Chat?)null);

        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        // Assert
        Assert.Equal("No such chat", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.MessageRepository.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenChatIsClosed_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new SendMessageRequest(new(), "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat { IsActive = false });

        // Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        // Assert
        Assert.Equal("This chat is closed", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.MessageRepository.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenChatIsExistsAndOpened_ShouldAddMessageAndReturnIt()
    {
        //Arrange
        var request = new SendMessageRequest(new(), "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat { IsActive = true });

        var messageRepositoryMock = new Mock<IRepository<Message>>();
        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.MessageRepository)
            .Returns(messageRepositoryMock.Object);

        _mapperMock.Setup(mapper => mapper.Map<Message>(It.IsAny<SendMessageRequest>()))
            .Returns(new Message());

        _mapperMock.Setup(mapper => mapper.Map<MessageDTO>(It.IsAny<Message>()))
            .Returns(new MessageDTO());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.MessageRepository.AddAsync(It.IsAny<Message>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
