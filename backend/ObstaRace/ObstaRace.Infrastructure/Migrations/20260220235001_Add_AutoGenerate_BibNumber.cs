using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_AutoGenerate_BibNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_BibNumber",
                table: "Registrations");

            migrationBuilder.AlterColumn<int>(
                name: "BibNumber",
                table: "Registrations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BibNumber",
                table: "Registrations",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_BibNumber",
                table: "Registrations",
                column: "BibNumber",
                unique: true);
        }
    }
}
