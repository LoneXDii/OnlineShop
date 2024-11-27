﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductsService.Infrastructure.Data;

#nullable disable

namespace ProductsService.Infrastructure.Migrations
{
    [DbContext(typeof(CommandDbContext))]
    partial class CommandDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("ProductsService.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

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
                        },
                        new
                        {
                            Id = 3,
                            Name = "Brand",
                            NormalizedName = "smartphones_brand",
                            ParentId = 1
                        },
                        new
                        {
                            Id = 4,
                            Name = "Brand",
                            NormalizedName = "laptops_brand",
                            ParentId = 2
                        },
                        new
                        {
                            Id = 5,
                            Name = "RAM",
                            NormalizedName = "smartphones_ram",
                            ParentId = 1
                        },
                        new
                        {
                            Id = 6,
                            Name = "RAM",
                            NormalizedName = "laptops_ram",
                            ParentId = 2
                        },
                        new
                        {
                            Id = 7,
                            Name = "ROM",
                            NormalizedName = "smartphones_rom",
                            ParentId = 1
                        },
                        new
                        {
                            Id = 8,
                            Name = "ROM",
                            NormalizedName = "laptops_rom",
                            ParentId = 2
                        },
                        new
                        {
                            Id = 9,
                            Name = "Processor",
                            NormalizedName = "laptops_processor",
                            ParentId = 2
                        },
                        new
                        {
                            Id = 10,
                            Name = "Samsung",
                            NormalizedName = "smartphones_brand_samsung",
                            ParentId = 3
                        },
                        new
                        {
                            Id = 11,
                            Name = "Apple",
                            NormalizedName = "smartphones_brand_apple",
                            ParentId = 3
                        },
                        new
                        {
                            Id = 12,
                            Name = "12GB",
                            NormalizedName = "smartphones_ram_12gb",
                            ParentId = 5
                        },
                        new
                        {
                            Id = 13,
                            Name = "8GB",
                            NormalizedName = "smartphones_ram_8gb",
                            ParentId = 5
                        },
                        new
                        {
                            Id = 14,
                            Name = "512GB",
                            NormalizedName = "smartphones_rom_512gb",
                            ParentId = 7
                        },
                        new
                        {
                            Id = 15,
                            Name = "Apple",
                            NormalizedName = "laptops_brand_apple",
                            ParentId = 4
                        },
                        new
                        {
                            Id = 16,
                            Name = "16GB",
                            NormalizedName = "laptops_ram_16gb",
                            ParentId = 6
                        },
                        new
                        {
                            Id = 17,
                            Name = "18GB",
                            NormalizedName = "laptops_ram_18gb",
                            ParentId = 6
                        },
                        new
                        {
                            Id = 18,
                            Name = "512GB",
                            NormalizedName = "laptops_rom_512gb",
                            ParentId = 8
                        },
                        new
                        {
                            Id = 19,
                            Name = "1024GB",
                            NormalizedName = "laptops_ram_1024gb",
                            ParentId = 8
                        },
                        new
                        {
                            Id = 20,
                            Name = "M3",
                            NormalizedName = "laptops_processor_m3",
                            ParentId = 9
                        },
                        new
                        {
                            Id = 21,
                            Name = "M3 Pro",
                            NormalizedName = "laptops_processor_m3pro",
                            ParentId = 9
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.CategoryProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductId");

                    b.ToTable("CategoryProducts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 3,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 5,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 4,
                            CategoryId = 7,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 5,
                            CategoryId = 10,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 6,
                            CategoryId = 12,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 7,
                            CategoryId = 14,
                            ProductId = 1
                        },
                        new
                        {
                            Id = 8,
                            CategoryId = 1,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 9,
                            CategoryId = 3,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 10,
                            CategoryId = 5,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 11,
                            CategoryId = 7,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 12,
                            CategoryId = 11,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 13,
                            CategoryId = 13,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 14,
                            CategoryId = 14,
                            ProductId = 2
                        },
                        new
                        {
                            Id = 15,
                            CategoryId = 2,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 16,
                            CategoryId = 4,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 17,
                            CategoryId = 6,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 18,
                            CategoryId = 8,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 19,
                            CategoryId = 9,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 20,
                            CategoryId = 15,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 21,
                            CategoryId = 17,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 22,
                            CategoryId = 19,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 23,
                            CategoryId = 21,
                            ProductId = 3
                        },
                        new
                        {
                            Id = 24,
                            CategoryId = 2,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 25,
                            CategoryId = 4,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 26,
                            CategoryId = 6,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 27,
                            CategoryId = 8,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 28,
                            CategoryId = 9,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 29,
                            CategoryId = 15,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 30,
                            CategoryId = 16,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 31,
                            CategoryId = 18,
                            ProductId = 4
                        },
                        new
                        {
                            Id = 32,
                            CategoryId = 20,
                            ProductId = 4
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

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Samsung Galaxy S24 Ultra",
                            Price = 1119.99,
                            Quantity = 14
                        },
                        new
                        {
                            Id = 2,
                            Name = "IPhone 15 Pro",
                            Price = 999.0,
                            Quantity = 10
                        },
                        new
                        {
                            Id = 3,
                            Name = "MacBook Pro 16'",
                            Price = 2499.0,
                            Quantity = 2
                        },
                        new
                        {
                            Id = 4,
                            Name = "MacBook Air 13'",
                            Price = 1099.0,
                            Quantity = 12
                        });
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Category", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.CategoryProduct", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Category", "Category")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductsService.Domain.Entities.Product", "Product")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Discount", b =>
                {
                    b.HasOne("ProductsService.Domain.Entities.Product", "Product")
                        .WithMany("Discounts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Category", b =>
                {
                    b.Navigation("CategoryProducts");

                    b.Navigation("Children");
                });

            modelBuilder.Entity("ProductsService.Domain.Entities.Product", b =>
                {
                    b.Navigation("CategoryProducts");

                    b.Navigation("Discounts");
                });
#pragma warning restore 612, 618
        }
    }
}
