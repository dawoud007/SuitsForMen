using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class updateCloths : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7393bb5a-1074-4d2f-95ed-105f2531a667");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "2b6a6610-8810-41f2-aff6-0479d348620c");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b6fb14c4-c571-44f2-b96c-4ec6752532ec", new DateTime(2023, 8, 6, 16, 17, 54, 108, DateTimeKind.Local).AddTicks(983), "AQAAAAEAACcQAAAAEAWpmgshopUav/RYmIQlyugIxzXGSY1FSjIwjdlleyFIf1LO1+hgAz///9B7BykgRQ==", "5faf9439-8237-455b-838f-ab160e3edc6c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "971db86d-ebb6-4e29-8144-17eeffcf9e33");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c69847ac-549d-499b-aba4-dc450bd8a256");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95be6fa0-1be4-478f-9657-1b3ea8a52678", new DateTime(2023, 8, 6, 16, 16, 39, 975, DateTimeKind.Local).AddTicks(8187), "AQAAAAEAACcQAAAAEPptESbbUm3Q74LmfctoyuLOh0SKBjNltPUe93XI8Gwx+Vzvg7zBL+lHo47WOh7ysw==", "f0f5ec2d-f7a8-4ce7-9d3f-c70ca1f213bc" });
        }
    }
}
