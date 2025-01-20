using Moq;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Tests.Setups;

public static class UnitOfWorkMockSetup
{
    public static void SetupUnitOfWork(this Mock<IUnitOfWork> mock)
    {
        mock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        mock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.DeleteAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        mock.Setup(unitOfWork =>
                unitOfWork.CategoryCommandRepository.UpdateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }
}