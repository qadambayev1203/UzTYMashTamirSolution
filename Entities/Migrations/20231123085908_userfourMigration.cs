using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class userfourMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dataLog",
                table: "users",
                newName: "datalog");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "datalog",
                table: "users",
                newName: "dataLog");
        }
    }
}
