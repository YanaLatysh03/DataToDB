using Microsoft.EntityFrameworkCore.Migrations;

namespace DataToDb.Migrations
{
    public partial class AddColumnValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Value",
                table: "LastBlock",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "LastBlock");
        }
    }
}
