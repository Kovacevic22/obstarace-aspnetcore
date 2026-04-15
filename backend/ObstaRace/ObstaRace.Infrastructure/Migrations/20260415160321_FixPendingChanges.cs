using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Participants_UserId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_ParticipantUserId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_UserId_RaceId",
                table: "Registrations");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ParticipantUserId_RaceId",
                table: "Registrations",
                columns: new[] { "ParticipantUserId", "RaceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserId",
                table: "Registrations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_ParticipantUserId_RaceId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_UserId",
                table: "Registrations");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ParticipantUserId",
                table: "Registrations",
                column: "ParticipantUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserId_RaceId",
                table: "Registrations",
                columns: new[] { "UserId", "RaceId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Participants_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "Participants",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Users_UserId",
                table: "Registrations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
