﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsService.Infrastructure.Data;

#nullable disable

namespace ProductsService.Infrastructure.Migrations
{
    [DbContext(typeof(CommandDbContext))]
    [Migration("20241125070611_FixProductAttributeTable")]
    partial class FixProductAttributeTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProductsService.Domain.Entities.Attribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Attributes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Brand",
                            NormalizedName = "phone_brand"
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            Name = "Brand",
                            NormalizedName = "laptop_brand"
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 1,
                            Name = "RAM",
                            NormalizedName = "phone_ram"
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            Name = "RAM",
                            NormalizedName = "laptop_ram"
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 1,
                            Name = "ROM",
                            NormalizedName = "phone_rom"
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 2,
                            Name = "ROM",
                            NormalizedName = "laptop_rom"
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 2,
                            Name = "Processor",
                            NormalizedName = "laptop_processor"
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Smartphones",
                            NormalizedName = "smartphones"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Laptops",
                            NormalizedName = "laptops"
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActived")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Percent")
                        .HasColumnType("int");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Discounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            EndDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            IsActived = false,
                            Percent = 15,
                            ProductId = 3,
                            StartDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            Name = "Samsung Galaxy S24 Ultra",
                            Price = 1119.99,
                            Quantity = 14
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 1,
                            Name = "IPhone 15 Pro",
                            Price = 999.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 2,
                            Name = "MacBook Pro 16'",
                            Price = 2499.0,
                            Quantity = 2
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 2,
                            Name = "MacBook Air 13'",
                            Price = 1099.0,
                            Quantity = 12
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.ProductAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AttributeId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductAttributes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AttributeId = 1,
                            ProductId = 1,
                            Value = "Samsung"
                        },
                        new
                        {
                            Id = 2,
                            AttributeId = 3,
                            ProductId = 1,
                            Value = "12GB"
                        },
                        new
                        {
                            Id = 3,
                            AttributeId = 5,
                            ProductId = 1,
                            Value = "512Gb"
                        },
                        new
                        {
                            Id = 4,
                            AttributeId = 1,
                            ProductId = 2,
                            Value = "Apple"
                        },
                        new
                        {
                            Id = 5,
                            AttributeId = 3,
                            ProductId = 2,
                            Value = "8GB"
                        },
                        new
                        {
                            Id = 6,
                            AttributeId = 5,
                            ProductId = 2,
                            Value = "512Gb"
                        },
                        new
                        {
                            Id = 7,
                            AttributeId = 2,
                            ProductId = 3,
                            Value = "Apple"
                        },
                        new
                        {
                            Id = 8,
                            AttributeId = 4,
                            ProductId = 3,
                            Value = "18GB"
                        },
                        new
                        {
                            Id = 9,
                            AttributeId = 6,
                            ProductId = 3,
                            Value = "1024Gb"
                        },
                        new
                        {
                            Id = 10,
                            AttributeId = 7,
                            ProductId = 3,
                            Value = "M3 Pro"
                        },
                        new
                        {
                            Id = 11,
                            AttributeId = 2,
                            ProductId = 4,
                            Value = "Apple"
                        },
                        new
                        {
                            Id = 12,
                            AttributeId = 4,
                            ProductId = 4,
                            Value = "16GB"
                        },
                        new
                        {
                            Id = 13,
                            AttributeId = 6,
                            ProductId = 4,
                            Value = "512Gb"
                        },
                        new
                        {
                            Id = 14,
                            AttributeId = 7,
                            ProductId = 4,
                            Value = "M3"
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Attribute", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Category", "Category")
                        .WithMany("Attributes")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Discount", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Product", "Product")
                        .WithMany("Discounts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Product", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.ProductAttribute", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Attribute", "Attribute")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("AttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductsService.Domain.Entities.Product", "Product")
                        .WithMany("ProductAttributes")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attribute");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Attribute", b =>
                {
                    b.Navigation("ProductAttributes");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Category", b =>
                {
                    b.Navigation("Attributes");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Product", b =>
                {
                    b.Navigation("Discounts");

                    b.Navigation("ProductAttributes");
                });
#pragma warning restore 612, 618
        }
    }
}
