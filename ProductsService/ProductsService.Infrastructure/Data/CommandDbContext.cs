using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Infrastructure.Data;

internal class CommandDbContext : DbContext
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<Discount> Discounts { get; set; }

    //OnModelCreating will be added later
}
