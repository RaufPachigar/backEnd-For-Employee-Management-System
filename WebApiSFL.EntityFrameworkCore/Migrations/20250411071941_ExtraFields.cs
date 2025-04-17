using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSFL.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class ExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateofJoining",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateofJoining",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");
        }
    }
}
