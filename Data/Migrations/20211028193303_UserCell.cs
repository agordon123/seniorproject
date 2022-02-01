using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class UserCell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CellProvider",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CellProvider",
                table: "AspNetUsers");
        }
    }
}
