using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OAT.Migrations
{
    /// <inheritdoc />
    public partial class CMK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable("documents");
			migrationBuilder.DropTable("CMK_News");
			migrationBuilder.DropTable("CMK");
            migrationBuilder.CreateTable(
                name: "CMK",
                columns: table => new
                {
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    compositions = table.Column<string>(type: "TEXT", nullable: false),
                    history = table.Column<string>(type: "TEXT", nullable: false),
                    descriptionOfWork = table.Column<string>(type: "TEXT", nullable: false),
                    achievements = table.Column<string>(type: "TEXT", nullable: false),
                    plans = table.Column<string>(type: "TEXT", nullable: false)
                });
			migrationBuilder.Sql("ALTER TABLE `CMK` ADD `id` int AUTO_INCREMENT PRIMARY KEY FIRST;");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
