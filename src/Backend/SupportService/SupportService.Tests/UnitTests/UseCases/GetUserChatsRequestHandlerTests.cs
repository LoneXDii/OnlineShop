using AutoMapper;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.UseCases.GetUserChats;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.UseCases;

public class GetUserChatsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetUserChatsRequestHandler _handler;

    public GetUserChatsRequestHandlerTests()
    {
        _handler = new GetUserChatsRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldReturnChatsList()
    {
        //Arrange
        var request = new GetUserChatsRequest("1");

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.ChatRepository.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Chat>());

        _mapperMock.Setup(mapper => mapper.Map<List<ChatDTO>>(It.IsAny<IEnumerable<Chat>>()))
            .Returns(new List<ChatDTO>());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
    }
}
