using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductsCqrs.Application.Abstractions.Data;
using ProductsCqrs.Application.Common;
using ProductsCqrs.Application.Products;
using ProductsCqrs.Domain.Products;
using ProductsCqrs.Infrastructure.Persistence;

namespace ProductsCqrs.Infrastructure.ReadModels;

internal sealed class ProductReadStore(ProductsDbContext db) : IProductReadStore
{
    private static readonly Expression<Func<Product, ProductResponse>> ToResponse = product => new ProductResponse(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Stock,
        product.CreatedAt,
        product.UpdatedAt);

    public Task<ProductResponse?> GetByIdAsync(int id, CancellationToken ct = default)
        => db.Products
            .AsNoTracking()
            .Where(product => product.Id == id)
            .Select(ToResponse)
            .FirstOrDefaultAsync(ct);

    public async Task<PagedResult<ProductResponse>> ListAsync(string? search, int page, int pageSize, CancellationToken ct = default)
    {
        var products = db.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowerCaseSearch = search.Trim().ToLowerInvariant();
            products = products.Where(product => product.Name.ToLower().Contains(lowerCaseSearch));
        }

        var totalCount = await products.CountAsync(ct);

        var items = await products
            .OrderBy(product => product.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(ToResponse)
            .ToListAsync(ct);

        return new PagedResult<ProductResponse>(items, page, pageSize, totalCount);
    }
}
