using Microsoft.EntityFrameworkCore;
using System;

namespace ProductsService.Infrastructure.Data;

internal class QueryDbContext : DbContext
{
    public QueryDbContext(DbContextOptions<QueryDbContext> options) : base(options)
    {
    }

    //OnModelCreating will be added later
}
