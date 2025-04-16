using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMAVIAT.Migrations
{
    /// <inheritdoc />
    public partial class EloVK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EloVkPostId",
                table: "News",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EloVkPostId",
                table: "News");
        }
    }
}
