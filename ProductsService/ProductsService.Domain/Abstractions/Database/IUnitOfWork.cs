using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Domain.Abstractions.Database;

public interface IUnitOfWork
{
    ICommandRepository<Product> ProductCommandRepository { get; }
    IQueryRepository<Product> ProductQueryRepository { get; }
    ICommandRepository<Category> CategoryCommandRepository { get; }
    IQueryRepository<Category> CategoryQueryRepository { get; }
    ICommandRepository<Attribute> AttributeCommandRepository { get; }
    IQueryRepository<Attribute> AttributeQueryRepository { get; }
    ICommandRepository<ProductAttribute> ProductAttributeCommandRepository { get; }
    IQueryRepository<ProductAttribute> ProductAttributeQueryRepository { get; }
    ICommandRepository<Discount> DiscountCommandRepository { get; }
    IQueryRepository<Discount> DiscountQueryRepository { get; }
    public Task SaveAllAsync();
    public Task DeleteDatabaseAsync();
    public Task CreateDatabaseAsync();

}
