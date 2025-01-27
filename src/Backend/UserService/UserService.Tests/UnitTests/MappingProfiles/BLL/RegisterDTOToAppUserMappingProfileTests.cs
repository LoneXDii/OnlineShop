using AutoFixture;
using AutoMapper;
using UserService.BLL.DTO;
using UserService.BLL.Mapping;
using UserService.DAL.Entities;

namespace UserService.Tests.UnitTests.MappingProfiles.BLL;

public class RegisterDTOToAppUserMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new();

    public RegisterDTOToAppUserMappingProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RegisterDTOToAppUserMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_RegisterDTOToAppUser_ShouldMapCorrectly()
    {
        //Arrange
        var registerDTO = _fixture.Build<RegisterDTO>()
            .Without(dto => dto.Avatar)
            .Create();

        //Act
        var appUser = _mapper.Map<AppUser>(registerDTO);

        //Assert
        Assert.NotNull(appUser);
        Assert.Equal(registerDTO.Email, appUser.UserName);
        Assert.Equal(registerDTO.FirstName, appUser.FirstName); 
        Assert.Equal(registerDTO.LastName, appUser.LastName); 
    }

    [Fact]
    public void Map_NullRegisterDTOToAppUser_ShouldReturnNull()
    {
        //Act
        var appUser = _mapper.Map<AppUser>(null);

        //Assert
        Assert.Null(appUser);
    }
}
