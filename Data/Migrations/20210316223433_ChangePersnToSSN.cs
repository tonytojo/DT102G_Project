using Microsoft.EntityFrameworkCore.Migrations;

namespace Slask.Data.Migrations
{
    public partial class ChangePersnToSSN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonNumber",
                table: "Orders",
                newName: "SSN");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SSN",
                table: "Orders",
                newName: "PersonNumber");
        }
    }
}
