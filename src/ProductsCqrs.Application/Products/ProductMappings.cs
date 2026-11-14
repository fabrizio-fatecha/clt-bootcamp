using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products;

internal static class ProductMappings
{
    public static ProductResponse ToResponse(this Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Stock,
        product.CreatedAt,
        product.UpdatedAt);
}
