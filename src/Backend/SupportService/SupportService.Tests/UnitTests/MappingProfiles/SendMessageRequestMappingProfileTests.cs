using AutoFixture;
using AutoMapper;
using SupportService.Application.DTO;
using SupportService.Application.Mapping;
using SupportService.Application.UseCases.SendMessage;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.MappingProfiles;

public class SendMessageRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public SendMessageRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<SendMesageRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenSendMessageRequestIsNotNull_ShouldReturnCorrectMessage()
    {
        //Arrange
        var request = _fixture.Create<SendMessageRequest>();

        //Act
        var message = _mapper.Map<Message>(request);

        //Assert
        Assert.NotNull(message);
        Assert.Equal(request.UserId, message.SenderId);
        Assert.Equal(request.Message.Text, message.Text);
        Assert.Equal(request.Message.ChatId, message.ChatId);
    }

    [Fact]
    public void Map_WhenChatIsNull_ShouldReturnNull()
    {
        //Act
        var message = _mapper.Map<Message>(null);

        //Assert
        Assert.Null(message);
    }
}
