using ProductsCqrs.Application.Abstractions.Messaging;

namespace ProductsCqrs.Application.Products.Queries.GetProduct;

public sealed record GetProductQuery(int Id) : IQuery<ProductResponse>;
