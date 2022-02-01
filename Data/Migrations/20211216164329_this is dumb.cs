using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class thisisdumb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PanopticonUserId",
                table: "Todos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PanopticonUserId",
                table: "KanboardNote",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_PanopticonUserId",
                table: "Todos",
                column: "PanopticonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_KanboardNote_PanopticonUserId",
                table: "KanboardNote",
                column: "PanopticonUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanboardNote_AspNetUsers_PanopticonUserId",
                table: "KanboardNote",
                column: "PanopticonUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_AspNetUsers_PanopticonUserId",
                table: "Todos",
                column: "PanopticonUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanboardNote_AspNetUsers_PanopticonUserId",
                table: "KanboardNote");

            migrationBuilder.DropForeignKey(
                name: "FK_Todos_AspNetUsers_PanopticonUserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_PanopticonUserId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_KanboardNote_PanopticonUserId",
                table: "KanboardNote");

            migrationBuilder.DropColumn(
                name: "PanopticonUserId",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "PanopticonUserId",
                table: "KanboardNote");
        }
    }
}
