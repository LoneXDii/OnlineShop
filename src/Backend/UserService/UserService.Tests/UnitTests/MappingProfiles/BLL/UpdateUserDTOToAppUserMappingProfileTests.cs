using AutoFixture;
using AutoMapper;
using UserService.BLL.DTO;
using UserService.BLL.Mapping;
using UserService.DAL.Entities;

namespace UserService.Tests.UnitTests.MappingProfiles.BLL;

public class UpdateUserDTOToAppUserMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public UpdateUserDTOToAppUserMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<UpdateUserDTOToAppUserMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_UpdateUserDTOToAppUser_ShouldMapCorrectly()
    {
        // Arrange
        var updateUserDTO = _fixture.Build<UpdateUserDTO>()
            .Without(dto => dto.Avatar)
            .Create();

        // Act
        var appUser = _mapper.Map<AppUser>(updateUserDTO);

        // Assert
        Assert.NotNull(appUser);
        Assert.Equal(updateUserDTO.FirstName, appUser.FirstName);
        Assert.Equal(updateUserDTO.LastName, appUser.LastName);   
        Assert.Null(appUser.AvatarUrl); 
    }

    [Fact]
    public void Map_NullUpdateUserDTOToAppUser_ShouldReturnNull()
    {
        // Act
        var appUser = _mapper.Map<AppUser>(null);

        // Assert
        Assert.Null(appUser);
    }
}
