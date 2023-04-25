// using CoffeeSpace.OrderingApi.Domain.CustomerInfo;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
//
// namespace CoffeeSpace.OrderingApi.Persistance.EntityTypeConfigurations;
//
// public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
// {
//
//     public void Configure(EntityTypeBuilder<Address> builder)
//     {
//         builder.ToTable("Addresses")
//             .HasKey(x => x.Id);
//
//         builder.Property(x => x.City)
//             .IsRequired()
//             .IsUnicode(false)
//             .HasMaxLength(50);
//         
//         builder.Property(x => x.Country)
//             .IsRequired()
//             .IsUnicode(false)
//             .HasMaxLength(50);
//         
//         builder.Property(x => x.Street)
//             .IsRequired()
//             .IsUnicode(false)
//             .HasMaxLength(50);
//     }
// }