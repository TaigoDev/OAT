using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OAT.Migrations
{
    /// <inheritdoc />
    public partial class CMK_merge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "CMK",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

    }
}
