using CoffeeSpace.Data.Models.CustomerInfo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers")
            .HasKey(x => x.Id);

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Customer);

        builder.HasOne(x => x.PaymentInfo)
            .WithOne()
            .HasForeignKey<Customer>(x => x.PaymentId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasOne(x => x.Address)
            .WithOne()
            .HasForeignKey<Customer>(x => x.AddressId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName("Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50)
            .HasColumnName("Name");

        builder.Property(x => x.PictureUrl)
            .IsRequired(false)
            .IsUnicode(false)
            .HasColumnName("Picture Url");

        builder.Property(x => x.LastName)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50)
            .HasColumnName("Last Name");
        
        builder.Property(x => x.Email)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50)
            .HasColumnName("Email");

        builder.Property(x => x.EmailConfirmed)
            .IsRequired()
            .HasColumnType("BOOL")
            .HasColumnName("EmailConfirmed");
        
        builder.Property(x => x.Password)
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(50)
            .HasColumnName("Password");

        builder.Property(x => x.Birthday)
            .IsRequired()
            .HasColumnType("DATE");

        builder.HasIndex(x => x.Id)
            .IsUnique()
            .IsDescending();
    }
}
