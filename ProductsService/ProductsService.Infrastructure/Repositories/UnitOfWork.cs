using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Data;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CommandDbContext _commandDbContext;
    private readonly QueryDbContext _queryDbContext;

    public UnitOfWork(CommandDbContext commandDbContext, QueryDbContext queryDbContext,
        ICommandRepository<Product> productCommandRepository, IQueryRepository<Product> productQueryRepository,
        ICommandRepository<Category> categoryCommandRepository, IQueryRepository<Category> categoryQueryRepository,
        ICommandRepository<Attribute> attributeCommandRepository, IQueryRepository<Attribute> attributeQueryRepository,
        ICommandRepository<Discount> discountCommandRepository, IQueryRepository<Discount> discountQueryRepository,
        ICommandRepository<ProductAttribute> productAttributeCommandRepository,
        IQueryRepository<ProductAttribute> productAttributeQueryRepository)
    {
        _commandDbContext = commandDbContext;
        _queryDbContext = queryDbContext;

        ProductCommandRepository = productCommandRepository;
        ProductQueryRepository = productQueryRepository;
        CategoryCommandRepository = categoryCommandRepository;
        CategoryQueryRepository = categoryQueryRepository;
        AttributeCommandRepository = attributeCommandRepository;
        AttributeQueryRepository = attributeQueryRepository;
        DiscountCommandRepository = discountCommandRepository;
        DiscountQueryRepository = discountQueryRepository;
        ProductAttributeCommandRepository = productAttributeCommandRepository;
        ProductAttributeQueryRepository = productAttributeQueryRepository;
    }

    public ICommandRepository<Product> ProductCommandRepository { get; private set; }

    public IQueryRepository<Product> ProductQueryRepository { get; private set; }

    public ICommandRepository<Category> CategoryCommandRepository { get; private set; }

    public IQueryRepository<Category> CategoryQueryRepository { get; private set; }

    public ICommandRepository<Attribute> AttributeCommandRepository { get; private set; }

    public IQueryRepository<Attribute> AttributeQueryRepository { get; private set; }

    public ICommandRepository<Discount> DiscountCommandRepository { get; private set; }

    public IQueryRepository<Discount> DiscountQueryRepository { get; private set; }
    public ICommandRepository<ProductAttribute> ProductAttributeCommandRepository { get; private set; }
    public IQueryRepository<ProductAttribute> ProductAttributeQueryRepository { get; private set; }

    public async Task EnableMigrationsAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.SaveChangesAsync(cancellationToken);
    }
}
