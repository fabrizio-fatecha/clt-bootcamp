namespace ProductsCqrs.Domain.Products;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, int? excludedProductId, CancellationToken ct = default);
    void Add(Product product);
    void Remove(Product product);
}
