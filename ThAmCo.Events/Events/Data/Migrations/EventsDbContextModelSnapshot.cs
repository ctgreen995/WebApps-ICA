﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Events.Events.Data;

#nullable disable

namespace ThAmCo.Events.Events.Data.Migrations
{
    [DbContext(typeof(EventsDbContext))]
    partial class EventsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFirstAider")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Staff");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsFirstAider = false,
                            Name = "Rishi Sunak"
                        },
                        new
                        {
                            Id = 2,
                            IsFirstAider = false,
                            Name = "Boris Johnson"
                        },
                        new
                        {
                            Id = 3,
                            IsFirstAider = false,
                            Name = "Jeremy Hunt"
                        },
                        new
                        {
                            Id = 4,
                            IsFirstAider = false,
                            Name = "Suella Braverman"
                        },
                        new
                        {
                            Id = 5,
                            IsFirstAider = false,
                            Name = "Dominic Raab"
                        },
                        new
                        {
                            Id = 6,
                            IsFirstAider = false,
                            Name = "Michael Levelup Gove"
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventType")
                        .HasColumnType("TEXT");

                    b.Property<int>("FoodBooking")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfGuests")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reservation")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalAttendedGuests")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.EventStaff", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EventId")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeId", "EventId");

                    b.HasIndex("EventId");

                    b.ToTable("EventStaff");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Postcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.Property<string>("Telephone")
                        .HasColumnType("TEXT");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Guests");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "ahealey@gmail.com",
                            IsDeleted = false,
                            Name = "Adam Healey",
                            Postcode = "TS196HB",
                            Street = "11 Kilburn Avenue",
                            Telephone = "07694836281",
                            Town = "Fairfield"
                        },
                        new
                        {
                            Id = 2,
                            Email = "ehealey@gmail.com",
                            IsDeleted = false,
                            Name = "Emily Healey",
                            Postcode = "TS196HB",
                            Street = "11 Kilburn Avenue",
                            Telephone = "07349548361",
                            Town = "Fairfield"
                        },
                        new
                        {
                            Id = 3,
                            Email = "hnicholson@gmail.com",
                            IsDeleted = false,
                            Name = "Heather Nicholson",
                            Postcode = "TS236LK",
                            Street = "6 Owington Grove",
                            Telephone = "07948567392",
                            Town = "Billingham"
                        },
                        new
                        {
                            Id = 4,
                            Email = "cgreen@gmail.com",
                            IsDeleted = false,
                            Name = "Christian Green",
                            Postcode = "TS236LK",
                            Street = "6 Owington Grove",
                            Telephone = "07849573820",
                            Town = "Billingham"
                        },
                        new
                        {
                            Id = 5,
                            Email = "jcasey@gmail.com",
                            IsDeleted = false,
                            Name = "James Casey",
                            Postcode = "TS204QD",
                            Street = "3 Southfield Way",
                            Telephone = "07968564782",
                            Town = "Norton"
                        },
                        new
                        {
                            Id = 6,
                            Email = "ejones@gmail.com",
                            IsDeleted = false,
                            Name = "Estelle Jones",
                            Postcode = "TS204QD",
                            Street = "3 Southfield Way",
                            Telephone = "07946754321",
                            Town = "Norton"
                        },
                        new
                        {
                            Id = 7,
                            Email = "sodell@gmail.com",
                            IsDeleted = false,
                            Name = "Stuart O'Dell",
                            Postcode = "TS202DW",
                            Street = "5 Brentford Road",
                            Telephone = "07958940587",
                            Town = "Norton"
                        },
                        new
                        {
                            Id = 8,
                            Email = "lwellburn@gmail.com",
                            IsDeleted = false,
                            Name = "Lindsey Wellburn",
                            Postcode = "TS202DW",
                            Street = "5 Brentford Road",
                            Telephone = "07948593058",
                            Town = "Norton"
                        },
                        new
                        {
                            Id = 9,
                            Email = "sdavies@gmail.com",
                            IsDeleted = false,
                            Name = "Stephen Davies",
                            Postcode = "TS202HW",
                            Street = "3 Wellway Walk",
                            Telephone = "07960594839",
                            Town = "Norton"
                        },
                        new
                        {
                            Id = 10,
                            Email = "ndavies@gmail.com",
                            IsDeleted = false,
                            Name = "Nicole Davies",
                            Postcode = "TS202HW",
                            Street = "3 Wellway Walk",
                            Telephone = "07908967432",
                            Town = "Norton"
                        });
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.GuestBooking", b =>
                {
                    b.Property<string>("EventBookingId")
                        .HasColumnType("TEXT");

                    b.Property<int>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Attended")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventBookingId", "GuestId");

                    b.HasIndex("GuestId");

                    b.ToTable("GuestBookings");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.EventStaff", b =>
                {
                    b.HasOne("ThAmCo.Events.Events.Data.Employee", "Employee")
                        .WithMany("Events")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Events.Events.Data.Event", "Event")
                        .WithMany("Staff")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.GuestBooking", b =>
                {
                    b.HasOne("ThAmCo.Events.Events.Data.Event", "Event")
                        .WithMany("Guests")
                        .HasForeignKey("EventBookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ThAmCo.Events.Events.Data.Guest", "Guest")
                        .WithMany("Bookings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Employee", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Event", b =>
                {
                    b.Navigation("Guests");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("ThAmCo.Events.Events.Data.Guest", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
