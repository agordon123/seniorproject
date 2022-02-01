using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class ruhroh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Tasks",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Tasks",
                newName: "CreationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Tasks",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Tasks",
                newName: "EndDate");
        }
    }
}
