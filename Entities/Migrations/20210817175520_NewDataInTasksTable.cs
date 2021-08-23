using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class NewDataInTasksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "035e2bd8-77f3-447f-b09a-38306f5536a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bcfb6870-9775-4abe-b12c-8c494e15c006");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bcfb6870-9775-4abe-b12c-8c494e15c006", "3bf69ae5-611a-4055-bd0f-afd62848f485", "Viewer", "VIEWER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "035e2bd8-77f3-447f-b09a-38306f5536a7", "b08879a2-d7db-48c3-bdd0-d51b361dd94c", "Administrator", "ADMINISTRATOR" });
        }
    }
}
