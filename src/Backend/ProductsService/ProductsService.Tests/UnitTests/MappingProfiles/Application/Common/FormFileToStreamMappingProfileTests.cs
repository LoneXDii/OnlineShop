using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using ProductsService.Application.Mapping.Common;

namespace ProductsService.Tests.UnitTests.MappingProfiles.Application.Common;

public class FormFileToStreamMappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly Mock<Stream> _streamMock = new();
    private readonly Mock<IFormFile> _formFileMock = new();
    
    public FormFileToStreamMappingProfileTests()
    {
        _formFileMock.Setup(file => file.OpenReadStream()).Returns(_streamMock.Object);
        
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<FormFileToStreamMappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_WhenIFormFileIsNotNull_ShouldReturnStream()
    {
        //Act
        var result = _mapper.Map<Stream>(_formFileMock.Object);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(_streamMock.Object, result);
    }
    
    [Fact]
    public void Map_WhenIFormFileIsNull_ShouldReturnNull()
    {
        //Arrange
        IFormFile? mockFormFile = null;
        
        //Act
        var result = _mapper.Map<Stream>(mockFormFile);
        
        //Assert
        Assert.Null(result);
    }
}