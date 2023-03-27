using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ThAmCo.Events.Events.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EventType = table.Column<string>(type: "TEXT", nullable: true),
                    Reservation = table.Column<string>(type: "TEXT", nullable: true),
                    FoodBooking = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAttendedGuests = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    Postcode = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Town = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    IsFirstAider = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuestBookings",
                columns: table => new
                {
                    EventBookingId = table.Column<string>(type: "TEXT", nullable: false),
                    GuestId = table.Column<int>(type: "INTEGER", nullable: false),
                    Attended = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestBookings", x => new { x.EventBookingId, x.GuestId });
                    table.ForeignKey(
                        name: "FK_GuestBookings_Events_EventBookingId",
                        column: x => x.EventBookingId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuestBookings_Guests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventStaff",
                columns: table => new
                {
                    EventId = table.Column<string>(type: "TEXT", nullable: false),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStaff", x => new { x.EmployeeId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventStaff_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStaff_Staff_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "Email", "IsDeleted", "Name", "Postcode", "Street", "Telephone", "Town" },
                values: new object[,]
                {
                    { 1, "ahealey@gmail.com", false, "Adam Healey", "TS196HB", "11 Kilburn Avenue", "07694836281", "Fairfield" },
                    { 2, "ehealey@gmail.com", false, "Emily Healey", "TS196HB", "11 Kilburn Avenue", "07349548361", "Fairfield" },
                    { 3, "hnicholson@gmail.com", false, "Heather Nicholson", "TS236LK", "6 Owington Grove", "07948567392", "Billingham" },
                    { 4, "cgreen@gmail.com", false, "Christian Green", "TS236LK", "6 Owington Grove", "07849573820", "Billingham" },
                    { 5, "jcasey@gmail.com", false, "James Casey", "TS204QD", "3 Southfield Way", "07968564782", "Norton" },
                    { 6, "ejones@gmail.com", false, "Estelle Jones", "TS204QD", "3 Southfield Way", "07946754321", "Norton" },
                    { 7, "sodell@gmail.com", false, "Stuart O'Dell", "TS202DW", "5 Brentford Road", "07958940587", "Norton" },
                    { 8, "lwellburn@gmail.com", false, "Lindsey Wellburn", "TS202DW", "5 Brentford Road", "07948593058", "Norton" },
                    { 9, "sdavies@gmail.com", false, "Stephen Davies", "TS202HW", "3 Wellway Walk", "07960594839", "Norton" },
                    { 10, "ndavies@gmail.com", false, "Nicole Davies", "TS202HW", "3 Wellway Walk", "07908967432", "Norton" }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "IsFirstAider", "Name" },
                values: new object[,]
                {
                    { 1, false, "Rishi Sunak" },
                    { 2, false, "Boris Johnson" },
                    { 3, false, "Jeremy Hunt" },
                    { 4, false, "Suella Braverman" },
                    { 5, false, "Dominic Raab" },
                    { 6, false, "Michael Levelup Gove" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventStaff_EventId",
                table: "EventStaff",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestBookings_GuestId",
                table: "GuestBookings",
                column: "GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventStaff");

            migrationBuilder.DropTable(
                name: "GuestBookings");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Guests");
        }
    }
}
