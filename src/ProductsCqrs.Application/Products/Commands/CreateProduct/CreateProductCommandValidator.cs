using FluentValidation;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(150)
            .WithMessage("El nombre no puede superar los {MaxLength} caracteres.")
            .MustAsync(async (name, ct) => !await productRepository.NameExistsAsync(name, excludedProductId: null, ct))
            .WithMessage("Ya existe un producto con este nombre.");

        RuleFor(command => command.Description)
            .MaximumLength(500)
            .WithMessage("La descripción no puede superar los {MaxLength} caracteres.");

        RuleFor(command => command.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El precio no puede ser negativo.")
            .PrecisionScale(18, 2, ignoreTrailingZeros: true)
            .WithMessage("El precio admite como máximo {ExpectedPrecision} dígitos en total, con {ExpectedScale} decimales.");

        RuleFor(command => command.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo.");
    }
}
