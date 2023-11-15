using CoffeeSpace.PaymentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.PaymentService.Persistence.Configurations;

internal sealed class PaypalOrderInformationConfiguration : IEntityTypeConfiguration<PaypalOrderInformation>
{

    public void Configure(EntityTypeBuilder<PaypalOrderInformation> builder)
    {
        builder.ToTable("Paypal Orders")
            .HasKey(o => o.Id);
        
        builder.Property(o => o.Id)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(o => o.BuyerId)
            .IsRequired()
            .IsUnicode(false);

        builder.HasOne(o => o.PaypalOrder)
            .WithOne()
            .HasForeignKey<PaypalOrderInformation>(o => o.PaypalOrderId)
            .IsRequired();

        builder.HasIndex(o => o.Id)
            .IsUnique();

        builder.HasIndex(o => o.PaypalOrderId)
            .IsUnique();
    }
}