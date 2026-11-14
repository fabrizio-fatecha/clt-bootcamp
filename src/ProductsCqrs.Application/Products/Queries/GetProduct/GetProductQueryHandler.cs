using ProductsCqrs.Application.Abstractions.Data;
using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;

namespace ProductsCqrs.Application.Products.Queries.GetProduct;

internal sealed class GetProductQueryHandler(IProductReadStore readStore)
    : IQueryHandler<GetProductQuery, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductQuery query, CancellationToken ct)
        => await readStore.GetByIdAsync(query.Id, ct)
            ?? throw NotFoundException.Product(query.Id);
}
