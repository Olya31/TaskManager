using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class NewData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CronModel",
                table: "Tasks",
                newName: "Header");

            migrationBuilder.AddColumn<string>(
                name: "CronFormat",
                table: "Tasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Tasks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTasks",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4328e88d-90a0-4691-a9a5-1276b54b0209", "60bf2c71-0195-4319-8927-eda73ec367e0", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8ffed10c-51fc-44a3-b7fb-d4fa1fe2aeea", "bb5c8041-a009-4c85-ba41-84b46d518871", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4328e88d-90a0-4691-a9a5-1276b54b0209");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ffed10c-51fc-44a3-b7fb-d4fa1fe2aeea");

            migrationBuilder.DropColumn(
                name: "CronFormat",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "NumberOfTasks",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Header",
                table: "Tasks",
                newName: "CronModel");
        }
    }
}
