using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObstaRace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerifyCols : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationToken",
                table: "Participants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpiry",
                table: "Participants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                table: "Participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationToken",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpiry",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "EmailVerified",
                table: "Participants");
        }
    }
}
