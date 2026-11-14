using ProductsCqrs.Domain.Common;

namespace ProductsCqrs.Domain.Products;

public sealed class Product : Entity
{
    private Product() { }

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static Product Create(string name, string? description, decimal price, int stock, DateTime now)
    {
        var product = new Product { CreatedAt = now };
        product.ApplyDetails(name, description, price, stock);
        return product;
    }

    public void Update(string name, string? description, decimal price, int stock, DateTime now)
    {
        ApplyDetails(name, description, price, stock);
        UpdatedAt = now;
    }

    private void ApplyDetails(string name, string? description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("El nombre del producto es obligatorio.");

        if (price < 0)
            throw new DomainException("El precio del producto no puede ser negativo.");

        if (stock < 0)
            throw new DomainException("El stock del producto no puede ser negativo.");

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Price = price;
        Stock = stock;
    }
}
