﻿// <auto-generated />
using System;
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Back.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241206162215_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("Back.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Furniture"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Clothing"
                        });
                });

            modelBuilder.Entity("Back.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Siret")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            Name = "Client1",
                            Siret = "1234567890"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Market Rd",
                            Name = "Client2",
                            Siret = "0987654321"
                        });
                });

            modelBuilder.Entity("Back.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCommande")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Statut")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ClientId = 1,
                            DateCommande = new DateTime(2024, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2906),
                            ProductId = 1,
                            Quantity = 2,
                            Statut = "En attente"
                        },
                        new
                        {
                            Id = 2,
                            ClientId = 2,
                            DateCommande = new DateTime(2024, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2908),
                            ProductId = 3,
                            Quantity = 5,
                            Statut = "Livrée"
                        },
                        new
                        {
                            Id = 3,
                            ClientId = 1,
                            DateCommande = new DateTime(2024, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2910),
                            ProductId = 2,
                            Quantity = 1,
                            Statut = "Expédiée"
                        });
                });

            modelBuilder.Entity("Back.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePeremption")
                        .HasColumnType("TEXT");

                    b.Property<string>("Emplacement")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            DatePeremption = new DateTime(2026, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2815),
                            Emplacement = "A1",
                            Name = "Laptop",
                            Price = 1000m,
                            Quantity = 4
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            DatePeremption = new DateTime(2029, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2868),
                            Emplacement = "B2",
                            Name = "Chair",
                            Price = 150m,
                            Quantity = 25
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            DatePeremption = new DateTime(2025, 12, 6, 17, 22, 14, 899, DateTimeKind.Local).AddTicks(2872),
                            Emplacement = "C3",
                            Name = "T-shirt",
                            Price = 20m,
                            Quantity = 50
                        });
                });

            modelBuilder.Entity("Back.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "AQAAAAIAAYagAAAAEOsDsPD2SyTlTO4As7xIe5Eb5M3B9GuKzBOgHLoSAlb5SQQ7qY5uhyR6HWRwGAJTbg==",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Back.Models.Order", b =>
                {
                    b.HasOne("Back.Models.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Back.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Back.Models.Product", b =>
                {
                    b.HasOne("Back.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Back.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Back.Models.Client", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}