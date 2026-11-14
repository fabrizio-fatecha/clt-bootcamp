using ProductsCqrs.Application.Abstractions.Data;
using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;

namespace ProductsCqrs.Application.Products.Queries.ListProducts;

internal sealed class ListProductsQueryHandler(IProductReadStore readStore)
    : IQueryHandler<ListProductsQuery, PagedResult<ProductResponse>>
{
    public Task<PagedResult<ProductResponse>> Handle(ListProductsQuery query, CancellationToken ct)
        => readStore.ListAsync(query.Search, query.Page, query.PageSize, ct);
}
