using Microsoft.EntityFrameworkCore.Migrations;

namespace PanOpticon.Data.Migrations
{
    public partial class propertystuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLists_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobListId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTasks_JobLists_JobListId",
                        column: x => x.JobListId,
                        principalTable: "JobLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobLists_TaskId",
                table: "JobLists",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTasks_JobListId",
                table: "JobTasks",
                column: "JobListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobTasks");

            migrationBuilder.DropTable(
                name: "JobLists");
        }
    }
}
