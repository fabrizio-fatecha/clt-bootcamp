using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Domain.Common;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var product = Product.Create(command.Name, command.Description, command.Price, command.Stock, now);

        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(ct);

        return product.ToResponse();
    }
}
