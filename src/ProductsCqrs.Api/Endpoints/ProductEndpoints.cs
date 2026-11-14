using Microsoft.AspNetCore.Http.HttpResults;
using ProductsCqrs.Api.RateLimiting;
using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;
using ProductsCqrs.Application.Products;
using ProductsCqrs.Application.Products.Commands.CreateProduct;
using ProductsCqrs.Application.Products.Commands.DeleteProduct;
using ProductsCqrs.Application.Products.Commands.UpdateProduct;
using ProductsCqrs.Application.Products.Queries.GetProduct;
using ProductsCqrs.Application.Products.Queries.ListProducts;

namespace ProductsCqrs.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var products = app.MapGroup("/api/products")
            .WithTags("Productos")
            .ProducesProblem(StatusCodes.Status429TooManyRequests);

        products.MapGet("/", ListProducts)
            .WithName("ListProducts")
            .WithSummary("Lista los productos")
            .WithDescription("Permite buscar por nombre y paginar el resultado.")
            .RequireRateLimiting(RateLimitPolicies.Reads)
            .ProducesValidationProblem();

        products.MapGet("/{id:int}", GetProduct)
            .WithName("GetProduct")
            .WithSummary("Obtiene un producto por su id")
            .RequireRateLimiting(RateLimitPolicies.Reads)
            .ProducesProblem(StatusCodes.Status404NotFound);

        products.MapPost("/", CreateProduct)
            .WithName("CreateProduct")
            .WithSummary("Crea un producto")
            .WithDescription("El nombre no puede repetirse, sin distinguir mayúsculas de minúsculas.")
            .RequireRateLimiting(RateLimitPolicies.Writes)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        products.MapPut("/{id:int}", UpdateProduct)
            .WithName("UpdateProduct")
            .WithSummary("Actualiza un producto existente")
            .RequireRateLimiting(RateLimitPolicies.Writes)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);

        products.MapDelete("/{id:int}", DeleteProduct)
            .WithName("DeleteProduct")
            .WithSummary("Elimina un producto")
            .RequireRateLimiting(RateLimitPolicies.Writes)
            .ProducesProblem(StatusCodes.Status404NotFound);

        return app;
    }

    private static async Task<Ok<PagedResult<ProductResponse>>> ListProducts(
        [AsParameters] ListProductsQuery query,
        ISender sender,
        CancellationToken ct)
        => TypedResults.Ok(await sender.Send(query, ct));

    private static async Task<Ok<ProductResponse>> GetProduct(int id, ISender sender, CancellationToken ct)
        => TypedResults.Ok(await sender.Send(new GetProductQuery(id), ct));

    private static async Task<CreatedAtRoute<ProductResponse>> CreateProduct(
        CreateProductRequest request,
        ISender sender,
        CancellationToken ct)
    {
        var command = new CreateProductCommand(request.Name, request.Description, request.Price, request.Stock);
        var product = await sender.Send(command, ct);

        return TypedResults.CreatedAtRoute(product, "GetProduct", new { id = product.Id });
    }

    private static async Task<Ok<ProductResponse>> UpdateProduct(
        int id,
        UpdateProductRequest request,
        ISender sender,
        CancellationToken ct)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.Stock);

        return TypedResults.Ok(await sender.Send(command, ct));
    }

    private static async Task<NoContent> DeleteProduct(int id, ISender sender, CancellationToken ct)
    {
        await sender.Send(new DeleteProductCommand(id), ct);

        return TypedResults.NoContent();
    }
}

public sealed record CreateProductRequest(string Name, string? Description, decimal Price, int Stock);

public sealed record UpdateProductRequest(string Name, string? Description, decimal Price, int Stock);
