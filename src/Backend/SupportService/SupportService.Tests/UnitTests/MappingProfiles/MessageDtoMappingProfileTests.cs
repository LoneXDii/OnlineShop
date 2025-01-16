using AutoFixture;
using AutoMapper;
using SupportService.Application.DTO;
using SupportService.Application.Mapping;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.MappingProfiles;

public class MessageDtoMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public MessageDtoMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MessageDtoMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenMessageIsNotNullAndMessageChatIsNotNull_ShouldReturnCorrectMessageDto()
    {
        //Arrange
        var message = _fixture.Build<Message>()
            .With(message => message.Chat, _fixture.Build<Chat>()
                .Without(chat => chat.Messages)
                .Create())
            .Create();

        //Act
        var messageDto = _mapper.Map<MessageDTO>(message);

        //Assert
        Assert.NotNull(messageDto);
        Assert.Equal(message.SenderId, messageDto.SenderId);
        Assert.Equal(message.Text, messageDto.Text);
        Assert.Equal(message.DateTime, messageDto.DateTime);
        Assert.Equal(message.Chat.ClientId, messageDto.ChatOwnerId);
    }

    [Fact]
    public void Map_WhenMessageIsNotNullAndMessageChatIsNull_ShouldReturnCorrectMessageDtoWithEmptyChatOwnerId()
    {
        //Arrange
        var message = _fixture.Build<Message>()
            .Without(message => message.Chat)
            .Create();

        //Act
        var messageDto = _mapper.Map<MessageDTO>(message);

        //Assert
        Assert.NotNull(messageDto);
        Assert.Equal(message.SenderId, messageDto.SenderId);
        Assert.Equal(message.Text, messageDto.Text);
        Assert.Equal(message.DateTime, messageDto.DateTime);
        Assert.Equal("", messageDto.ChatOwnerId);
    }

    [Fact]
    public void Map_WhenMessageIsNull_ShouldReturnNull()
    {
        //Act
        var messageDto = _mapper.Map<MessageDTO>(null);

        //Assert
        Assert.Null(messageDto);
    }
}
