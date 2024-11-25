using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Entities;
using Attribute = ProductsService.Domain.Entities.Attribute;

namespace ProductsService.Infrastructure.Data;

//Here is no OnModelCreating, because this db is replicated from CommandDB
internal class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Attribute> Attributes { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
}
