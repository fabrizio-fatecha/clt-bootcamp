using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(string Name, string? Description, decimal Price, int Stock)
    : ICommand<ProductResponse>;
