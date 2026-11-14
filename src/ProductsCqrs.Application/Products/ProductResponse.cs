namespace ProductsCqrs.Application.Products;

public sealed record ProductResponse(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
