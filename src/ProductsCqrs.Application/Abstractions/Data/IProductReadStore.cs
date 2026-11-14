using ProductsCqrs.Application.Common;
using ProductsCqrs.Application.Products;

namespace ProductsCqrs.Application.Abstractions.Data;

public interface IProductReadStore
{
    Task<ProductResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<ProductResponse>> ListAsync(string? search, int page, int pageSize, CancellationToken ct = default);
}
