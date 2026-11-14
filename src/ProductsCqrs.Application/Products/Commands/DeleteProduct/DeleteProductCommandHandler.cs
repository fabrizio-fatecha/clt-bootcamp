using ProductsCqrs.Application.Abstractions.Messaging;
using ProductsCqrs.Application.Common;
using ProductsCqrs.Domain.Common;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products.Commands.DeleteProduct;

internal sealed class DeleteProductCommandHandler(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await productRepository.GetByIdAsync(command.Id, ct)
            ?? throw NotFoundException.Product(command.Id);

        productRepository.Remove(product);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
