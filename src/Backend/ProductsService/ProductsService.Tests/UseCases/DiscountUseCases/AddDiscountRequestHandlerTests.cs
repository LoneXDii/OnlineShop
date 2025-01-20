using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.Proxy;
using ProductsService.Application.Specifications.Discounts;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UseCases.DiscountUseCases;

public class AddDiscountRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<AddDiscountRequestHandler>> _loggerMock = new();
    private readonly Mock<IBackgroundJobProxy> _backgroundJobMock = new();
    private readonly Fixture _fixture = new();
    private readonly AddDiscountRequestHandler _handler;

    public AddDiscountRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();
        
        _handler = new AddDiscountRequestHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _loggerMock.Object,
            _backgroundJobMock.Object);
    }

    [Fact]
    public async Task Handle_WhenDiscountDoesNotExist_ShouldCreateDiscount()
    {
        //Arrange
        var request = _fixture.Create<AddDiscountRequest>();
        var discount = EntityFactory.CreateDiscount();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.DiscountQueryRepository.FirstOrDefaultAsync(It.IsAny<DiscountProductSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Discount?)null);

        _mapperMock.Setup(mapper => mapper.Map<Discount>(request))
            .Returns(discount);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.DiscountCommandRepository.AddAsync(discount, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);

        _backgroundJobMock.Verify(job => 
            job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDiscountAlreadyExists_ShouldThrowBadRequestException()
    {
        //Arrange
        var request = _fixture.Create<AddDiscountRequest>();
        var existingDiscount = EntityFactory.CreateDiscount();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.DiscountQueryRepository.FirstOrDefaultAsync(It.IsAny<DiscountProductSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingDiscount);

        //Act
        var exception = await Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(request, CancellationToken.None));
        
        //Assert
        Assert.Equal("This product already have discount", exception.Message);
        
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.DiscountCommandRepository.AddAsync(It.IsAny<Discount>(), It.IsAny<CancellationToken>()), Times.Never);
        
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);

        _backgroundJobMock.Verify(job => 
            job.Schedule(It.IsAny<Expression<Func<Task>>>(), It.IsAny<DateTime>()), Times.Never);
    }
}