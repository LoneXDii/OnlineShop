using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsService.Application.Exceptions;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Tests.Factories;
using ProductsService.Tests.Setups;

namespace ProductsService.Tests.UnitTests.UseCases.DiscountUseCases;

public class DeleteDiscountRequestHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteDiscountRequestHandler>> _loggerMock = new();
    private readonly Fixture _fixture = new();
    private readonly DeleteDiscountRequestHandler _handler;

    public DeleteDiscountRequestHandlerTests()
    {
        _unitOfWorkMock.SetupUnitOfWork();

        _handler = new DeleteDiscountRequestHandler(
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenDiscountExists_ShouldDeleteDiscount()
    {
        //Arrange
        var request = _fixture.Create<DeleteDiscountRequest>();
        var discount = EntityFactory.CreateDiscount();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.DiscountQueryRepository.GetByIdAsync(request.DiscountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(discount);

        //Act
        await _handler.Handle(request, CancellationToken.None);

        //Assert
        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.DiscountCommandRepository.DeleteAsync(discount, It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDiscountDoesNotExist_ShouldThrowNotFoundException()
    {
        //Arrange
        var discountId = _fixture.Create<int>();

        _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.DiscountQueryRepository.GetByIdAsync(discountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Discount?)null);

        var request = new DeleteDiscountRequest(discountId);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        //Assert
        Assert.Equal("No such discount", exception.Message);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.DiscountCommandRepository.DeleteAsync(It.IsAny<Discount>(), It.IsAny<CancellationToken>()), Times.Never);

        _unitOfWorkMock.Verify(unitOfWork =>
            unitOfWork.SaveAllAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}