using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;
using ProductsService.Infrastructure.Data;

namespace ProductsService.Infrastructure.Repositories;

internal class UnitOfWork : IUnitOfWork
{
    private readonly CommandDbContext _commandDbContext;
    private readonly QueryDbContext _queryDbContext;

    public UnitOfWork(CommandDbContext commandDbContext, QueryDbContext queryDbContext,
        ICommandRepository<Product> productCommandRepository, IQueryRepository<Product> productQueryRepository,
        ICommandRepository<Category> categoryCommandRepository, IQueryRepository<Category> categoryQueryRepository,
        ICommandRepository<Discount> discountCommandRepository, IQueryRepository<Discount> discountQueryRepository,
		ICommandRepository<CategoryProduct> categoryProductCommandRepository, IQueryRepository<CategoryProduct> categoryProductQueryRepository)
    {
        _commandDbContext = commandDbContext;
        _queryDbContext = queryDbContext;

        ProductCommandRepository = productCommandRepository;
        ProductQueryRepository = productQueryRepository;
        CategoryCommandRepository = categoryCommandRepository;
        CategoryQueryRepository = categoryQueryRepository;
        DiscountCommandRepository = discountCommandRepository;
        DiscountQueryRepository = discountQueryRepository;
        CategoryProductCommandRepository = categoryProductCommandRepository;
		CategoryProductQueryRepository = categoryProductQueryRepository;
    }

    public ICommandRepository<Product> ProductCommandRepository { get; private set; }

    public IQueryRepository<Product> ProductQueryRepository { get; private set; }

    public ICommandRepository<Category> CategoryCommandRepository { get; private set; }

    public IQueryRepository<Category> CategoryQueryRepository { get; private set; }

    public ICommandRepository<Discount> DiscountCommandRepository { get; private set; }

    public IQueryRepository<Discount> DiscountQueryRepository { get; private set; }
	public ICommandRepository<CategoryProduct> CategoryProductCommandRepository { get; private set; }
	public IQueryRepository<CategoryProduct> CategoryProductQueryRepository { get; private set; }

	public async Task EnableMigrationsAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.Database.MigrateAsync(cancellationToken);
    }

    public async Task SaveAllAsync(CancellationToken cancellationToken = default)
    {
        await _commandDbContext.SaveChangesAsync(cancellationToken);
    }
}
