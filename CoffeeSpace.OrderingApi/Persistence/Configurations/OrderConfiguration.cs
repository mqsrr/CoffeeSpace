using CoffeeSpace.Domain.Ordering.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StackExchange.Redis;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.OrderingApi.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(status => status.ToString(),
                value => Enum.Parse<OrderStatus>(value));

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Order>("AddressId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Id)
            .IsUnique();
    }
}