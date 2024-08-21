using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMAVIAT.Migrations
{
    /// <inheritdoc />
    public partial class ScheduleLib : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicLoads",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Group = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubGroupId = table.Column<int>(type: "int", nullable: false),
                    Discipline = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    CompletedHours = table.Column<double>(type: "double", nullable: false),
                    TotalHours = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicLoads", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
            migrationBuilder.DropColumn(
	            name: "bells",
	            table: "daysChanges");

            migrationBuilder.CreateTable(
		            name: "Bell",
		            columns: table => new
		            {
			            TableId = table.Column<int>(type: "int", nullable: false)
				            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
			            id = table.Column<string>(type: "longtext", nullable: true)
				            .Annotation("MySql:CharSet", "utf8mb4"),
			            period = table.Column<string>(type: "longtext", nullable: true)
				            .Annotation("MySql:CharSet", "utf8mb4"),
			            DaysChangesTableid = table.Column<int>(type: "int", nullable: true)
		            },
		            constraints: table =>
		            {
			            table.PrimaryKey("PK_Bell", x => x.TableId);
			            table.ForeignKey(
				            name: "FK_Bell_daysChanges_DaysChangesTableid",
				            column: x => x.DaysChangesTableid,
				            principalTable: "daysChanges",
				            principalColumn: "id");
		            })
	            .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
	            name: "IX_Bell_DaysChangesTableid",
	            table: "Bell",
	            column: "DaysChangesTableid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicLoads");
            migrationBuilder.DropTable(
	            name: "Bell");

            migrationBuilder.AddColumn<string>(
		            name: "bells",
		            table: "daysChanges",
		            type: "longtext",
		            nullable: false)
	            .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

