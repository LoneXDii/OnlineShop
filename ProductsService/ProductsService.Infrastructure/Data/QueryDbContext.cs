using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Entities;

namespace ProductsService.Infrastructure.Data;

//Here is no OnModelCreating, because this db is replicated from CommandDB
internal class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options) { }

	public DbSet<Product> Products { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<Discount> Discounts { get; set; }
	public DbSet<CategoryProduct> CategoryProducts { get; set; }
}
