using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(int Id, string Name, string? Description, decimal Price, int Stock)
    : ICommand<ProductResponse>;
