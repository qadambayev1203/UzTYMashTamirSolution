using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class usertreeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "organizationType",
                table: "users",
                newName: "organizationtype");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "organizationtype",
                table: "users",
                newName: "organizationType");
        }
    }
}
