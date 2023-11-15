using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Persistence.Configurations;

internal sealed class PurchaseUnitConfiguration : IEntityTypeConfiguration<PurchaseUnit>
{

    public void Configure(EntityTypeBuilder<PurchaseUnit> builder)
    {
        builder.ToTable("PurchaseUnits")
            .HasKey(pu => pu.Id);

        builder.Property(pu => pu.Id)
            .HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(pu => pu.Description)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(250);

        builder.Property(pu => pu.SoftDescriptor)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(pu => pu.CustomId)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.HasIndex(pu => pu.Id)
            .IsUnique();

        builder.Ignore(pu => pu.InvoiceId);

        builder.OwnsMany(pu => pu.Items, itemBuilder =>
        {
            itemBuilder.ToTable("Items");

            itemBuilder.Property(i => i.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            itemBuilder.Property(i => i.Description)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(250);

            itemBuilder.Property(i => i.Category)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);

            itemBuilder.Property(i => i.Quantity)
                .IsRequired()
                .IsUnicode(false);

            itemBuilder.Property(i => i.UnitAmount)
                .IsRequired()
                .IsUnicode(false);

            itemBuilder.Property(i => i.UnitAmount)
                .IsRequired();

            itemBuilder.Property(i => i.UnitAmount)
                .IsRequired()
                .HasConversion(money => money.Value,
                    value => new Money
                    {
                        CurrencyCode = "USD",
                        Value = value
                    });

            itemBuilder.Ignore(i => i.Sku);
            itemBuilder.Ignore(i => i.Tax);
        });
    }
}