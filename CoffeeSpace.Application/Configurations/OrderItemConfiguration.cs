using CoffeeSpace.Application.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.Application.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Title)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);
        
        builder.Property(x => x.Description)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(x => x.PictureUrl)
            .IsRequired();

        builder.Ignore(x => x.Quantity);
        builder.Ignore(x => x.Total);

        builder.Property(x => x.UnitPrice)
            .IsRequired()
            .HasPrecision(2);

        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending();
    }
}