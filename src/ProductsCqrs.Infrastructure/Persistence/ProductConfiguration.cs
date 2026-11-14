using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductsCqrs.Domain.Products;

namespace ProductsCqrs.Infrastructure.Persistence;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products", "public");

        builder.HasKey(product => product.Id);
        builder.Property(product => product.Id).HasColumnName("id");

        builder.Property(product => product.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(product => product.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(product => product.Price)
            .HasColumnName("price")
            .HasPrecision(18, 2);

        builder.Property(product => product.Stock).HasColumnName("stock");
        builder.Property(product => product.CreatedAt).HasColumnName("created_at");
        builder.Property(product => product.UpdatedAt).HasColumnName("updated_at");
    }
}
