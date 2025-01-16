using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.Exceptions;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;
using System.Linq.Expressions;

namespace SupportService.Tests.Application.UseCases;

public class CreateChatRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new(); 
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<CreateChatRequestHandler>> _loggerMock = new();
    private readonly CreateChatRequestHandler _handler;

    public CreateChatRequestHandlerTests()
    {
        _handler = new CreateChatRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenOpenedChatForThisUserAlreadyExists_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = new CreateChatRequest("A", "B");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Chat, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Chat());

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        //Assert
        Assert.Equal("This chat is already exists", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.AddAsync(It.IsAny<Chat>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserDontHaveOpenedChat_ShouldReturnNewChat()
    {
        //Arrange
        var request = new CreateChatRequest("Id", "Name");

        _unitOfWorkMock.Setup(unitOfWork =>
            unitOfWork.ChatRepository.FirstOrDefaultAsync(It.IsAny<Expression<Func<Chat, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Chat?)null);

        _mapperMock.Setup(mapper => mapper.Map<Chat>(It.IsAny<CreateChatRequest>()))
            .Returns(new Chat());

        _mapperMock.Setup(mapper => mapper.Map<ChatDTO>(It.IsAny<Chat>()))
            .Returns(new ChatDTO());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.ChatRepository.AddAsync(It.IsAny<Chat>(), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
