using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByInRace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Races",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Races_CreatedById",
                table: "Races",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Users_CreatedById",
                table: "Races",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Users_CreatedById",
                table: "Races");

            migrationBuilder.DropIndex(
                name: "IX_Races_CreatedById",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Races");
        }
    }
}
