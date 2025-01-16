using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Application.UseCases.GetChatById;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Tests.Application.UseCases;

public class GetChatByIdRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetChatByIdRequestHandler>> _loggerMock;
    private readonly GetChatByIdRequestHandler _handler;

    public GetChatByIdRequestHandlerTests()
    {
        _unitOfWorkMock = new();
        _mapperMock = new();
        _loggerMock = new();

        _handler = new GetChatByIdRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenChatIsNotExists_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new GetChatByIdRequest(1, "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Chat?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        //Assert
        Assert.Equal("No such chat", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoAccessToChat_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new GetChatByIdRequest(1, "1");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat { ClientId = "2" });

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        // Assert
        Assert.Equal("You have no access to this chat", exception.Message);
    }

    [Fact]
    public async Task Handle_WherUserIsChatOwner_ShouldReturnChat()
    {
        //Arrange
        var request = new GetChatByIdRequest(1, "1");
        var chat = new Chat { Id = 1, ClientId = "1", IsActive = true };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        _mapperMock.Setup(mapper => mapper.Map<ChatDTO>(It.IsAny<Chat>()))
            .Returns(new ChatDTO());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Handle_WherRequestedByAdmin_ShouldReturnClosedChat()
    {
        //Arrange
        var request = new GetChatByIdRequest(1, null);
        var chat = new Chat { Id = 1, ClientId = "1", IsActive = true };

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.GetByIdAsync(request.ChatId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(chat);

        _mapperMock.Setup(mapper => mapper.Map<ChatDTO>(It.IsAny<Chat>()))
            .Returns(new ChatDTO());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
    }
}
