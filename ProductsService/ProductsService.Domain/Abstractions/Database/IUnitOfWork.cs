using ProductsService.Domain.Entities;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Abstractions.Database;

public interface IUnitOfWork
{
    ICommandRepository<Product> ProductCommandRepository { get; }
    IQueryRepository<Product> ProductQueryRepository { get; }
    ICommandRepository<Category> CategoryCommandRepository { get; }
    IQueryRepository<Category> CategoryQueryRepository { get; }
    ICommandRepository<Discount> DiscountCommandRepository { get; }
    IQueryRepository<Discount> DiscountQueryRepository { get; }
    Task SaveAllAsync(CancellationToken cancellationToken = default);
    Task EnableMigrationsAsync(CancellationToken cancellationToken = default);
    void AttachInCommandContext(IEntity entity);
}
