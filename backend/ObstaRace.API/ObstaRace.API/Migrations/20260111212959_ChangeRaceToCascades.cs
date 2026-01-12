using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRaceToCascades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceObstacles_Races_RaceId",
                table: "RaceObstacles");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Races_RaceId",
                table: "Registrations");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceObstacles_Races_RaceId",
                table: "RaceObstacles",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Races_RaceId",
                table: "Registrations",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RaceObstacles_Races_RaceId",
                table: "RaceObstacles");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Races_RaceId",
                table: "Registrations");

            migrationBuilder.AddForeignKey(
                name: "FK_RaceObstacles_Races_RaceId",
                table: "RaceObstacles",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Races_RaceId",
                table: "Registrations",
                column: "RaceId",
                principalTable: "Races",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
