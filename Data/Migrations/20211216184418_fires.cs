using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class fires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PanopticonUserId",
                table: "Files",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_PanopticonUserId",
                table: "Files",
                column: "PanopticonUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_AspNetUsers_PanopticonUserId",
                table: "Files",
                column: "PanopticonUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_PanopticonUserId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_PanopticonUserId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "PanopticonUserId",
                table: "Files");
        }
    }
}
