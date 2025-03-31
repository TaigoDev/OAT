using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMAVIAT.Migrations
{
    /// <inheritdoc />
    public partial class TelegramElo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EloTelegramMediaGroupId",
                table: "News",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "EloTelegramMessageId",
                table: "News",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EloTelegramMediaGroupId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "EloTelegramMessageId",
                table: "News");
        }
    }
}
