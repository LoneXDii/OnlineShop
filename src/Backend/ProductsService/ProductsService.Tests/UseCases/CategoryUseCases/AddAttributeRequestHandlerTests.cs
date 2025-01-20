﻿using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.CategoryUseCases.Commands.AddAttribute;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.UseCases.CategoryUseCases;

public class AddAttributeRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<AddAttributeRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly AddAttributeRequestHandler _handler;
    
    public AddAttributeRequestHandlerTests()
    {
        _handler = new AddAttributeRequestHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }
    
    [Fact]
    public async Task Handle_WhenParentCategoryDoesNotExist_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeRequest>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.ParentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, default));

        //Assert
        Assert.Equal("No such parent", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenParentCategoryExists_ShouldAddAttribute()
    {
        //Arrange
        var request = _fixture.Create<AddAttributeRequest>();
        var category = _fixture.Build<Category>()
            .Without(c => c.Parent)
            .Without(c => c.Products)
            .Without(c => c.Children)
            .Create();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryQueryRepository.GetByIdAsync(request.ParentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.AddAsync(category, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _mapperMock.Setup(mapper => mapper.Map<Category>(It.IsAny<AddAttributeRequest>()))
            .Returns(category);

        //Act
        await _handler.Handle(request, default);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.CategoryCommandRepository.AddAsync(category, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}