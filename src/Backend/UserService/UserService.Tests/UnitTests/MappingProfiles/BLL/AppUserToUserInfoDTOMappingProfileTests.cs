using AutoFixture;
using AutoMapper;
using UserService.BLL.DTO;
using UserService.BLL.Mapping;
using UserService.DAL.Entities;

namespace UserService.Tests.UnitTests.MappingProfiles.BLL;

public class AppUserToUserInfoDTOMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public AppUserToUserInfoDTOMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppUserToUserInfoDTOMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_AppUserToUserInfoDTO_ShouldMapCorrectly()
    {
        //Arrange
        var appUser = _fixture.Create<AppUser>();

        //Act
        var userInfoDTO = _mapper.Map<UserInfoDTO>(appUser);

        //Assert
        Assert.NotNull(userInfoDTO);
        Assert.Equal(appUser.Id, userInfoDTO.Id);
        Assert.Equal(appUser.FirstName, userInfoDTO.FirstName);
        Assert.Equal(appUser.LastName, userInfoDTO.LastName);
        Assert.Equal(appUser.Email, userInfoDTO.Email);
        Assert.Equal(appUser.AvatarUrl, userInfoDTO.Avatar);
    }

    [Fact]
    public void Map_AppUserToUserWithRolesDTO_ShouldMapCorrectly()
    {
        //Arrange
        var appUser = _fixture.Create<AppUser>();
        var roles = _fixture.CreateMany<string>(3).ToList();

        //Act
        var userWithRolesDTO = _mapper.Map<UserWithRolesDTO>(appUser);

        //Assert
        Assert.NotNull(userWithRolesDTO);
        Assert.Equal(appUser.Id, userWithRolesDTO.Id);
        Assert.Equal(appUser.FirstName, userWithRolesDTO.FirstName);
        Assert.Equal(appUser.LastName, userWithRolesDTO.LastName);
        Assert.Equal(appUser.Email, userWithRolesDTO.Email);
        Assert.Equal(appUser.AvatarUrl, userWithRolesDTO.Avatar); 
    }

    [Fact]
    public void Map_NullAppUserToUserInfoDTO_ShouldReturnNull()
    {
        //Act
        var userInfoDTO = _mapper.Map<UserInfoDTO>(null);

        //Assert
        Assert.Null(userInfoDTO);
    }

    [Fact]
    public void Map_NullAppUserToUserWithRolesDTO_ShouldReturnNull()
    {
        //Act
        var userWithRolesDTO = _mapper.Map<UserWithRolesDTO>(null);

        //Assert
        Assert.Null(userWithRolesDTO);
    }
}
