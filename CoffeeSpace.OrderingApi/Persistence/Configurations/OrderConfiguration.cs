using CoffeeSpace.Domain.Ordering.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.EntityTypeConfigurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders")
            .HasKey(x => x.Id);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Order>(x => x.AddressId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.PaymentInfo)
            .WithOne()
            .HasForeignKey<Order>(x => x.PaymentInfoId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Buyer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.BuyerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Id)
            .IsUnicode(false)
            .IsRequired();
        
        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending(false);
    }
}