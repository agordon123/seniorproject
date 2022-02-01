using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class todos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanboardNote_Todo_TodoId",
                table: "KanboardNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todo",
                table: "Todo");

            migrationBuilder.RenameTable(
                name: "Todo",
                newName: "Todos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todos",
                table: "Todos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KanboardNote_Todos_TodoId",
                table: "KanboardNote",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanboardNote_Todos_TodoId",
                table: "KanboardNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Todos",
                table: "Todos");

            migrationBuilder.RenameTable(
                name: "Todos",
                newName: "Todo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Todo",
                table: "Todo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KanboardNote_Todo_TodoId",
                table: "KanboardNote",
                column: "TodoId",
                principalTable: "Todo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
