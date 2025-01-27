using AutoFixture;
using AutoMapper;
using SupportService.Application.Mapping;
using SupportService.Application.UseCases.CreateChat;
using SupportService.Domain.Entities;

namespace SupportService.Tests.UnitTests.MappingProfiles;

public class CreateChatRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public CreateChatRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CreateChatRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenChatIsNotNull_ShouldReturnCorrectChatDto()
    {
        //Arrange
        var request = _fixture.Create<CreateChatRequest>();

        //Act
        var chat = _mapper.Map<Chat>(request);

        //Assert
        Assert.NotNull(chat);
        Assert.Equal(request.ClientId, chat.ClientId);
        Assert.Equal(request.ClientName, chat.ClientName);
        Assert.True(chat.IsActive);
    }

    [Fact]
    public void Map_WhenChatIsNull_ShouldReturnNull()
    {
        //Act
        var chat = _mapper.Map<Chat>(null);

        //Assert
        Assert.Null(chat);
    }
}
