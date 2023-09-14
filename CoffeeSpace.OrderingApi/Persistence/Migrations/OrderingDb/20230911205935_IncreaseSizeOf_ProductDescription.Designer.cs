﻿// <auto-generated />
using CoffeeSpace.OrderingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.OrderingDb
{
    [DbContext(typeof(OrderingDbContext))]
    [Migration("20230911205935_IncreaseSizeOf_ProductDescription")]
    partial class IncreaseSizeOf_ProductDescription
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.BuyerInfo.Address", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Addresses", (string)null);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.BuyerInfo.Buyer", b =>
                {
                    b.Property<string>("Id")
                        .IsUnicode(false)
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique()
                        .IsDescending();

                    b.ToTable("Buyers", (string)null);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo.PaymentInfo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("CardType")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("ExpirationMonth")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<int>("ExpirationYear")
                        .IsUnicode(false)
                        .HasColumnType("integer");

                    b.Property<string>("SecurityNumber")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Payments", (string)null);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.Orders.Order", b =>
                {
                    b.Property<string>("Id")
                        .IsUnicode(false)
                        .HasColumnType("text");

                    b.Property<string>("AddressId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("BuyerId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentInfoId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique();

                    b.HasIndex("BuyerId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PaymentInfoId")
                        .IsUnique();

                    b.ToTable("Orders", (string)null);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.Orders.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .IsUnicode(false)
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("character varying(200)");

                    b.Property<float>("Discount")
                        .HasPrecision(2)
                        .IsUnicode(false)
                        .HasColumnType("real");

                    b.Property<string>("OrderId")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .IsUnicode(false)
                        .HasColumnType("character varying(64)");

                    b.Property<float>("UnitPrice")
                        .HasPrecision(2)
                        .IsUnicode(false)
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems", (string)null);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.Orders.Order", b =>
                {
                    b.HasOne("CoffeeSpace.Domain.Ordering.BuyerInfo.Address", "Address")
                        .WithOne()
                        .HasForeignKey("CoffeeSpace.Domain.Ordering.Orders.Order", "AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoffeeSpace.Domain.Ordering.BuyerInfo.Buyer", null)
                        .WithMany("Orders")
                        .HasForeignKey("BuyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo.PaymentInfo", "PaymentInfo")
                        .WithOne()
                        .HasForeignKey("CoffeeSpace.Domain.Ordering.Orders.Order", "PaymentInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("PaymentInfo");
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.Orders.OrderItem", b =>
                {
                    b.HasOne("CoffeeSpace.Domain.Ordering.Orders.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.BuyerInfo.Buyer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("CoffeeSpace.Domain.Ordering.Orders.Order", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}
