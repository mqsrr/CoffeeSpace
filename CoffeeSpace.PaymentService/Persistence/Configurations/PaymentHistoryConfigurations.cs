using CoffeeSpace.PaymentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.PaymentService.Persistence.Configurations;

internal sealed class PaymentHistoryConfigurations : IEntityTypeConfiguration<PaymentHistory>
{
    public void Configure(EntityTypeBuilder<PaymentHistory> builder)
    {
        builder.ToTable("Payment_History")
            .HasKey(x => x.Id);

        builder.Property(x => x.OrderDate)
            .IsRequired();

        builder.Property(x => x.OrderId)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(64);
        
        builder.Property(x => x.PaymentId)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(64);

        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending(false);
    }
}