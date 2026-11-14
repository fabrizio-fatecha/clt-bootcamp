using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;
using ProductsCqrs.Domain.Common;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : ICommandHandler<UpdateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(command.Id, ct)
            ?? throw NotFoundException.Product(command.Id);

        var now = timeProvider.GetUtcNow().UtcDateTime;

        product.Update(command.Name, command.Description, command.Price, command.Stock, now);

        await unitOfWork.SaveChangesAsync(ct);

        return product.ToResponse();
    }
}
