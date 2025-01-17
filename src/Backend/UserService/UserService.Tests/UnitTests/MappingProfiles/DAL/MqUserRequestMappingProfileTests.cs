using AutoFixture;
using AutoMapper;
using UserService.DAL.Entities;
using UserService.DAL.Mapping;
using UserService.DAL.Models;

namespace UserService.Tests.UnitTests.MappingProfiles.DAL;

public class MqUserRequestMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public MqUserRequestMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MqUserRequestMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_AppUserToMqUserRequest_ShouldMapCorrectly()
    {
        //Arrange
        var appUser = _fixture.Create<AppUser>();

        //Act
        var mqUserRequest = _mapper.Map<MqUserRequest>(appUser);

        //Assert
        Assert.NotNull(mqUserRequest);
        Assert.Equal(appUser.Id, mqUserRequest.Id);
        Assert.Equal(appUser.Email, mqUserRequest.Email); 
        Assert.Equal($"{appUser.FirstName} {appUser.LastName}", mqUserRequest.Name); 
    }

    [Fact]
    public void Map_NullAppUserToMqUserRequest_ShouldReturnNull()
    {
        //Act
        var mqUserRequest = _mapper.Map<MqUserRequest>(null);

        //Assert
        Assert.Null(mqUserRequest);
    }
}
