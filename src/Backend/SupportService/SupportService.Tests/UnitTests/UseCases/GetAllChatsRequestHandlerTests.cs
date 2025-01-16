using AutoMapper;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.UseCases.GetAllChats;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.UseCases;

public class GetAllChatsRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly GetAllChatsRequestHandler _handler;

    public GetAllChatsRequestHandlerTests()
    {
        _handler = new GetAllChatsRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldReturnChatsList()
    {
        //Arrange
        var request = new GetAllChatsRequest();

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
