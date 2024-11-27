using ProductsService.Domain.Entities;

namespace ProductsService.Domain.Abstractions.Database;

public interface IUnitOfWork
{
	ICommandRepository<Product> ProductCommandRepository { get; }
    IQueryRepository<Product> ProductQueryRepository { get; }
    ICommandRepository<Category> CategoryCommandRepository { get; }
    IQueryRepository<Category> CategoryQueryRepository { get; }
    ICommandRepository<Discount> DiscountCommandRepository { get; }
    IQueryRepository<Discount> DiscountQueryRepository { get; }
	ICommandRepository<CategoryProduct> CategoryProductCommandRepository { get; }
	IQueryRepository<CategoryProduct> CategoryProductQueryRepository { get; }
    Task SaveAllAsync(CancellationToken cancellationToken = default);
    Task EnableMigrationsAsync(CancellationToken cancellationToken = default);
}
