using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class ChangesToNotificationPart2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskNotificationSchedules_TaskNotificationSchedulesId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_TaskNotificationSchedulesId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "TaskNotificationSchedulesId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "TaskNotificationSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TaskNotificationSchedules_TaskId",
                table: "TaskNotificationSchedules",
                column: "TaskId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskNotificationSchedules_Tasks_TaskId",
                table: "TaskNotificationSchedules",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskNotificationSchedules_Tasks_TaskId",
                table: "TaskNotificationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_TaskNotificationSchedules_TaskId",
                table: "TaskNotificationSchedules");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "TaskNotificationSchedules");

            migrationBuilder.AddColumn<int>(
                name: "TaskNotificationSchedulesId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskNotificationSchedulesId",
                table: "Tasks",
                column: "TaskNotificationSchedulesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskNotificationSchedules_TaskNotificationSchedulesId",
                table: "Tasks",
                column: "TaskNotificationSchedulesId",
                principalTable: "TaskNotificationSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
