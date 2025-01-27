using AutoFixture;
using MediatR;
using Moq;
using ProductsService.API.Controllers;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.AddDiscount;
using ProductsService.Application.UseCases.DiscountUseCases.Commands.DeleteDiscount;

namespace ProductsService.Tests.UnitTests.Controllers;

public class DiscountsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly DiscountsController _controller;
    private readonly Fixture _fixture = new();
    
    public DiscountsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new DiscountsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task AddDiscount_WhenCalled_ShouldCallAddDiscountRequest()
    {
        //Arrange
        var request = _fixture.Create<AddDiscountRequest>();

        //Act
        await _controller.AddDiscount(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task DeleteDiscount_WhenCalled_ShouldCallDeleteDiscountRequest()
    {
        //Arrange
        var request = _fixture.Create<DeleteDiscountRequest>();

        //Act
        await _controller.DeleteDiscount(request, CancellationToken.None);

        //Assert
        _mediatorMock.Verify(m => m.Send(request, CancellationToken.None), Times.Once);
    }
}