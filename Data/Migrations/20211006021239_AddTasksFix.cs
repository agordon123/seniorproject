using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class AddTasksFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PanopticonUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskPriority = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskCss = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_PanopticonUserId",
                        column: x => x.PanopticonUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskNotificationSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    Thirty = table.Column<bool>(type: "bit", nullable: false),
                    HourBefore = table.Column<bool>(type: "bit", nullable: false),
                    None = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskNotificationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskNotificationSchedules_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskNotificationSchedules_TaskId",
                table: "TaskNotificationSchedules",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PanopticonUserId",
                table: "Tasks",
                column: "PanopticonUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskNotificationSchedules");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
