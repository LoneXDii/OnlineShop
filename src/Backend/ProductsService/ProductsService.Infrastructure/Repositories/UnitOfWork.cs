using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Domain.Entities.Abstractions;
using ProductsService.Infrastructure.Data;

namespace ProductsService.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CommandDbContext _commandDbContext;
    private readonly QueryDbContext _queryDbContext;

    public UnitOfWork(CommandDbContext commandDbContext, QueryDbContext queryDbContext,
        ICommandRepository<Product> productCommandRepository, IQueryRepository<Product> productQueryRepository,
        ICommandRepository<Category> categoryCommandRepository, IQueryRepository<Category> categoryQueryRepository,
        ICommandRepository<Discount> discountCommandRepository, IQueryRepository<Discount> discountQueryRepository)
    {
        _commandDbContext = commandDbContext;
        _queryDbContext = queryDbContext;

        ProductCommandRepository = productCommandRepository;
        ProductQueryRepository = productQueryRepository;
        CategoryCommandRepository = categoryCommandRepository;
        CategoryQueryRepository = categoryQueryRepository;
        DiscountCommandRepository = discountCommandRepository;
        DiscountQueryRepository = discountQueryRepository;
    }

    public ICommandRepository<Product> ProductCommandRepository { get; private set; }

    public IQueryRepository<Product> ProductQueryRepository { get; private set; }

    public ICommandRepository<Category> CategoryCommandRepository { get; private set; }

    public IQueryRepository<Category> CategoryQueryRepository { get; private set; }

    public ICommandRepository<Discount> DiscountCommandRepository { get; private set; }

    public IQueryRepository<Discount> DiscountQueryRepository { get; private set; }

    public async Task EnableMigrationsAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.SaveChangesAsync(cancellationToken);
    }

    public void AttachInCommandContext(params IEntity[] entities)
    {
        foreach (var entity in entities)
        {
            _commandDbContext.Attach(entity);
        }
    }

    public void AttachInCommandContext(IEnumerable<IEntity> entities)
    {
        foreach (var entity in entities)
        {
            _commandDbContext.Attach(entity);
        }
    }
}
