using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : ICommand;
