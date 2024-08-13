using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.PaymentService.Application.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.PaymentService.Persistence.Configurations;

internal sealed class PaypalOrderInformationConfiguration : IEntityTypeConfiguration<PaypalOrderInformation>
{

    public void Configure(EntityTypeBuilder<PaypalOrderInformation> builder)
    {
        builder.ToTable("Paypal Orders")
            .HasKey(o => o.Id);

        builder.Property(o => o.BuyerId)
            .IsRequired();

        builder.HasOne(o => o.PaypalOrder)
            .WithOne()
            .HasForeignKey<PaypalOrderInformation>("PaypalOrderId")
            .IsRequired();

        builder.HasIndex(o => o.Id)
            .IsUnique();

        builder.HasIndex("PaypalOrderId")
            .IsUnique();
    }
}