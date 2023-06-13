using CoffeeSpace.Domain.Ordering.BuyerInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.Configurations;

public sealed class BuyerConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable("Buyers")
            .HasKey(x => x.Id);

        builder.HasMany(x => x.Orders)
            .WithOne()
            .HasForeignKey(x => x.BuyerId);

        builder.Property(x => x.Id)
            .IsRequired()
            .IsUnicode(false);

        builder.Property(x => x.Name)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50);
        
        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending();
    }
}
