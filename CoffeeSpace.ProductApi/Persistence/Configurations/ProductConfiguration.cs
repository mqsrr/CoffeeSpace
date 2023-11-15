using CoffeeSpace.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.ProductApi.Persistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products")
            .HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .HasMaxLength(64)
            .IsUnicode(false)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(200)
            .IsUnicode(false)
            .IsRequired();
        
        builder.Property(x => x.UnitPrice)
            .IsUnicode(false)
            .HasPrecision(2)
            .IsRequired();
        
        builder.Property(x => x.Discount)
            .IsUnicode(false)
            .HasPrecision(2)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsUnicode(false)
            .IsRequired();
        
        builder.Ignore(x => x.Total);

        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending(false);
    }
}