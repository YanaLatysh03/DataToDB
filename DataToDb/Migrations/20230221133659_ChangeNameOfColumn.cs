using Microsoft.EntityFrameworkCore.Migrations;

namespace DataToDb.Migrations
{
    public partial class ChangeNameOfColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_id",
                table: "Blocks",
                newName: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Blocks",
                newName: "_id");
        }
    }
}
