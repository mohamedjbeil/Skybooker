using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookService.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PassengerName",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Bookings",
                newName: "PassengerLastname");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "Bookings",
                newName: "PassengerFirstname");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "UpdatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketCount",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TicketCount",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Bookings",
                newName: "BookingDate");

            migrationBuilder.RenameColumn(
                name: "PassengerLastname",
                table: "Bookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "PassengerFirstname",
                table: "Bookings",
                newName: "SeatNumber");

            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PassengerName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
