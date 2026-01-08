using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Races_Slug",
                table: "Races",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Races_Slug",
                table: "Races");
        }
    }
}
