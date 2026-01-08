using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedSomeAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Obstacles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Obstacles_CreatedById",
                table: "Obstacles",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Obstacles_Users_CreatedById",
                table: "Obstacles",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Obstacles_Users_CreatedById",
                table: "Obstacles");

            migrationBuilder.DropIndex(
                name: "IX_Obstacles_CreatedById",
                table: "Obstacles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Obstacles");
        }
    }
}
