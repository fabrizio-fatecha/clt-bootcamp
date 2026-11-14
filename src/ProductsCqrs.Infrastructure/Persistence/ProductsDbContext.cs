using Microsoft.EntityFrameworkCore;
using ProductsCqrs.Domain.Common;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Infrastructure.Persistence;

public sealed class ProductsDbContext(DbContextOptions<ProductsDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
}
