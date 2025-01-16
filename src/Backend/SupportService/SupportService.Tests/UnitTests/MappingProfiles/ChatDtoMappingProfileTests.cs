using AutoFixture;
using AutoMapper;
using SupportService.Application.DTO;
using SupportService.Application.Mapping;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.MappingProfiles;

public class ChatDtoMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public ChatDtoMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ChatDtoMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenChatIsNotNull_ShouldReturnCorrectChatDto()
    {
        //Arrange
        var chat = _fixture.Build<Chat>()
            .Without(chat => chat.Messages)
            .Create();

        //Act
        var chatDto = _mapper.Map<ChatDTO>(chat);

        //Assert
        Assert.NotNull(chatDto);
        Assert.Equal(chat.Id, chatDto.Id);
        Assert.Equal(chat.ClientId, chatDto.ClientId);
        Assert.Equal(chat.ClientName, chatDto.ClientName);
        Assert.Equal(chat.IsActive, chatDto.IsActive);
    }

    [Fact]
    public void Map_WhenChatIsNull_ShouldReturnNull()
    {
        //Act
        var chatDto = _mapper.Map<ChatDTO>(null);

        //Assert
        Assert.Null(chatDto);
    }
}
