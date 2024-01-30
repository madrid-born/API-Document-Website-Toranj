using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_DOC.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Methods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CsCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Php1Code = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Php2Code = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PythonCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NodeJsCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Methods", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Methods");
        }
    }
}
