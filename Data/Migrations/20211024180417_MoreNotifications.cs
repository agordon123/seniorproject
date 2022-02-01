using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class MoreNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Fifteen",
                table: "TaskNotificationSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FiveHours",
                table: "TaskNotificationSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OneDay",
                table: "TaskNotificationSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fifteen",
                table: "TaskNotificationSchedules");

            migrationBuilder.DropColumn(
                name: "FiveHours",
                table: "TaskNotificationSchedules");

            migrationBuilder.DropColumn(
                name: "OneDay",
                table: "TaskNotificationSchedules");
        }
    }
}
