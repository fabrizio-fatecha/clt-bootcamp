using FluentValidation;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Application.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Id)
            .GreaterThan(0)
            .WithMessage("El id del producto debe ser mayor a cero.");

        RuleFor(command => command.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(150)
            .WithMessage("El nombre no puede superar los {MaxLength} caracteres.")
            .MustAsync(async (command, name, ct) => !await productRepository.NameExistsAsync(name, command.Id, ct))
            .WithMessage("Ya existe otro producto con este nombre.");

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
