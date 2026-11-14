using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;

namespace ProductsCqrs.Application.Products.Queries.ListProducts;

public sealed record ListProductsQuery(string? Search, int Page = 1, int PageSize = 10)
    : IQuery<PagedResult<ProductResponse>>;
