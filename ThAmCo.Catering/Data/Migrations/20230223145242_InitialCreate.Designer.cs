﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Catering.Data;

#nullable disable

namespace ThAmCo.Catering.Data.Migrations
{
    [DbContext(typeof(CateringDbContext))]
    [Migration("20230223145242_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodBooking", b =>
                {
                    b.Property<int>("FoodBookingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClientReferenceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("MenuId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("INTEGER");

                    b.HasKey("FoodBookingId");

                    b.HasIndex("MenuId");

                    b.ToTable("FoodBookings");

                    b.HasData(
                        new
                        {
                            FoodBookingId = 123,
                            ClientReferenceId = "Green",
                            MenuId = 1,
                            NumberOfGuests = 10
                        });
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("FoodItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Enchiladas and Chips",
                            UnitPrice = 4.0
                        },
                        new
                        {
                            Id = 2,
                            Description = "Tacos and Chips",
                            UnitPrice = 6.0
                        },
                        new
                        {
                            Id = 3,
                            Description = "Egg and Chips",
                            UnitPrice = 5.0
                        },
                        new
                        {
                            Id = 4,
                            Description = "Sunday Dinner and Chips",
                            UnitPrice = 10.0
                        },
                        new
                        {
                            Id = 5,
                            Description = "Broccoli and Chips",
                            UnitPrice = 8.0
                        },
                        new
                        {
                            Id = 6,
                            Description = "Avacado and Chips",
                            UnitPrice = 7.0
                        });
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Menu", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Menus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Veggie"
                        },
                        new
                        {
                            Id = 2,
                            Name = "English"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Mexican"
                        });
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFoodItem", b =>
                {
                    b.Property<int>("MenuId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MenuId", "FoodItemId");

                    b.HasIndex("FoodItemId");

                    b.ToTable("MenuFoodItems");

                    b.HasData(
                        new
                        {
                            MenuId = 1,
                            FoodItemId = 5
                        },
                        new
                        {
                            MenuId = 1,
                            FoodItemId = 6
                        },
                        new
                        {
                            MenuId = 2,
                            FoodItemId = 3
                        },
                        new
                        {
                            MenuId = 2,
                            FoodItemId = 4
                        },
                        new
                        {
                            MenuId = 3,
                            FoodItemId = 1
                        },
                        new
                        {
                            MenuId = 3,
                            FoodItemId = 2
                        });
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodBooking", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.Menu", "Menu")
                        .WithMany("FoodBookings")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.MenuFoodItem", b =>
                {
                    b.HasOne("ThAmCo.Catering.Data.FoodItem", "FoodItem")
                        .WithMany("Menus")
                        .HasForeignKey("FoodItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Catering.Data.Menu", "Menu")
                        .WithMany("MenuFoodItems")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoodItem");

                    b.Navigation("Menu");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.FoodItem", b =>
                {
                    b.Navigation("Menus");
                });

            modelBuilder.Entity("ThAmCo.Catering.Data.Menu", b =>
                {
                    b.Navigation("FoodBookings");

                    b.Navigation("MenuFoodItems");
                });
#pragma warning restore 612, 618
        }
    }
}
