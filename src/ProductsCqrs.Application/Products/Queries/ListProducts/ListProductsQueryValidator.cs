using FluentValidation;

namespace ProductsCqrs.Application.Products.Queries.ListProducts;

internal sealed class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
{
    public ListProductsQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("La página debe ser mayor o igual a 1.");

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre {From} y {To}.");
    }
}
