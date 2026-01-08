using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedObstaclesIdInRaceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ObstacleIds",
                table: "Races",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObstacleIds",
                table: "Races");
        }
    }
}
