using AutoMapper;
using Moq;
using SupportService.Application.DTO;
using SupportService.Application.UseCases.GetChatMessages;
using SupportService.Domain.Abstractions;
using SupportService.Domain.Entities;

namespace SupportService.Tests.Application.UseCases;

public class GetChatMessagesRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetChatMessagesRequestHandler _handler;

    public GetChatMessagesRequestHandlerTests()
    {
        _unitOfWorkMock = new();
        _mapperMock = new();

        _handler = new GetChatMessagesRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenRequested_ShouldReturnMessagesList()
    {
        //Arrange
        var request = new GetChatMessagesRequest(1);

        _unitOfWorkMock.Setup(unitOfWork => unitOfWork.MessageRepository.ListAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Message>());

        _mapperMock.Setup(mapper => mapper.Map<List<MessageDTO>>(It.IsAny<IEnumerable<Message>>()))
            .Returns(new List<MessageDTO>());

        //Act
        var result = await _handler.Handle(request, default);

        //Assert
        Assert.NotNull(result);
    }
}
