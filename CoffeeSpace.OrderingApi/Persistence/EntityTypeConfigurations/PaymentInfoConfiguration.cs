using CoffeeSpace.Domain.Ordering.CustomerInfo.CardInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.EntityTypeConfigurations;

public sealed class PaymentInfoConfiguration : IEntityTypeConfiguration<PaymentInfo>
{

    public void Configure(EntityTypeBuilder<PaymentInfo> builder)
    {
        builder.ToTable("Payments")
            .HasKey(x => x.Id);

        builder.Property(x => x.CardType)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(x => x.CardNumber)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(x => x.ExpirationMonth)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(x => x.ExpirationYear)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(x => x.SecurityNumber)
            .IsRequired()
            .IsUnicode(false);
    }
}