using Moq;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.Setups;

public static class UnitOfWorkMockSetup
{
    public static void SetupUnitOfWork(this Mock<IUnitOfWork> mock)
    {
        var categoryCommandRepositoryMock = new Mock<ICommandRepository<Category>>();
        mock.Setup(uow => uow.CategoryCommandRepository).Returns(categoryCommandRepositoryMock.Object);
        
        var categoryQueryRepository = new Mock<IQueryRepository<Category>>();
        mock.Setup(uow => uow.CategoryQueryRepository).Returns(categoryQueryRepository.Object);
        
        var productCommandRepositoryMock = new Mock<ICommandRepository<Product>>();
        mock.Setup(uow => uow.ProductCommandRepository).Returns(productCommandRepositoryMock.Object);
        
        var productQueryRepository = new Mock<IQueryRepository<Product>>();
        mock.Setup(uow => uow.ProductQueryRepository).Returns(productQueryRepository.Object);
        
        var discountCommandRepositoryMock = new Mock<ICommandRepository<Discount>>();
        mock.Setup(uow => uow.DiscountCommandRepository).Returns(discountCommandRepositoryMock.Object);
        
        var discountQueryRepository = new Mock<IQueryRepository<Discount>>();
        mock.Setup(uow => uow.DiscountQueryRepository).Returns(discountQueryRepository.Object);
    }
}