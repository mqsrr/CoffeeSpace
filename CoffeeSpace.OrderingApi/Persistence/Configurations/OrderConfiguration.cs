using CoffeeSpace.Domain.Ordering.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders")
            .HasKey(x => x.Id);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Order>("AddressId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.PaymentInfo)
            .WithOne()
            .HasForeignKey<Order>("PaymentInfoId")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id)
            .IsUnicode(false)
            .IsRequired();
        
        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending(false);
    }
}