using CoffeeSpace.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.ProductApi.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products")
            .HasKey(product => product.Id);
        
        builder.Property(product => product.Title)
            .HasMaxLength(64)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(product => product.Image)
            .IsRequired()
            .IsUnicode(false);
        
        builder.Property(product => product.Description)
            .HasMaxLength(200)
            .IsUnicode(false)
            .IsRequired();
        
        builder.Property(product => product.UnitPrice)
            .IsUnicode(false)
            .HasPrecision(2)
            .IsRequired();

        builder.Property(product => product.Quantity)
            .IsUnicode(false)
            .IsRequired();
        
        builder.Ignore(product => product.Total);

        builder.HasIndex(product => product.Id)
            .IsUnique()
            .IsDescending(false);
    }
}