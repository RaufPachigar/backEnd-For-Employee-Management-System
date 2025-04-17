using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiSFL.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class positiontorole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Employees",
                newName: "Role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Employees",
                newName: "Position");
        }
    }
}
