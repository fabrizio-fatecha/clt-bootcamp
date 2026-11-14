using Microsoft.EntityFrameworkCore;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Infrastructure.Persistence;

internal sealed class ProductRepository(ProductsDbContext db) : IProductRepository
{
    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Products.FirstOrDefaultAsync(product => product.Id == id, ct);

    public Task<bool> NameExistsAsync(string name, int? excludedProductId, CancellationToken ct = default)
    {
        var lowerCaseName = name.Trim().ToLowerInvariant();

        return db.Products.AnyAsync(
            product => product.Name.ToLower() == lowerCaseName && product.Id != excludedProductId,
            ct);
    }

    public void Add(Product product) => db.Products.Add(product);

    public void Remove(Product product) => db.Products.Remove(product);
}
