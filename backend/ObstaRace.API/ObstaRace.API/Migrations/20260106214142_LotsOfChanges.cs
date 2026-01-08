using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class LotsOfChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Registrations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Registrations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
