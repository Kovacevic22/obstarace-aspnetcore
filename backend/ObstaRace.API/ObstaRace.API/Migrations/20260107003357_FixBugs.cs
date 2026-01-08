using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class FixBugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObstacleIds",
                table: "Races");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObstacleIds",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
