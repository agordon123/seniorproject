using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class todo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TodoId",
                table: "KanboardNote",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Todo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KanboardNote_TodoId",
                table: "KanboardNote",
                column: "TodoId");

            migrationBuilder.AddForeignKey(
                name: "FK_KanboardNote_Todo_TodoId",
                table: "KanboardNote",
                column: "TodoId",
                principalTable: "Todo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KanboardNote_Todo_TodoId",
                table: "KanboardNote");

            migrationBuilder.DropTable(
                name: "Todo");

            migrationBuilder.DropIndex(
                name: "IX_KanboardNote_TodoId",
                table: "KanboardNote");

            migrationBuilder.DropColumn(
                name: "TodoId",
                table: "KanboardNote");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
