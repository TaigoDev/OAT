using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMAVIAT.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
            
	        migrationBuilder.AddColumn<string>(
			        name: "TypeOfLoad",
			        table: "AcademicLoads",
			        type: "longtext",
			        nullable: false)
		        .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Group = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Schedule = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PastCouples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    corpus = table.Column<int>(type: "int", nullable: false),
                    Group = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShortName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    subGroupId = table.Column<int>(type: "int", nullable: false),
                    CompletedHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastCouples", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
