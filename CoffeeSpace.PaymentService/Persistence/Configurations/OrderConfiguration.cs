using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{

    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders")
            .HasKey(o => o.Id);

        builder.Property(o => o.Status)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(o => o.CheckoutPaymentIntent)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(o => o.CreateTime)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(o => o.UpdateTime)
            .IsRequired(false)
            .IsUnicode(false);

        builder.Property(o => o.ExpirationTime)
            .IsRequired(false)
            .IsUnicode(false);

        builder.HasMany(o => o.PurchaseUnits)
            .WithOne();

        builder.Ignore(o => o.Payer);
        builder.Ignore(o => o.Links);
    }
}