using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class updateCloth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Clothes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Clothes");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "7ba59020-4c25-48f5-ab8b-0e3ff18b2cc9");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "36266f63-2a0e-45cc-a752-17f836bf85a8");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c35971b5-3d7e-4d0b-bb8e-79395ef3fab0", new DateTime(2023, 8, 6, 16, 12, 8, 578, DateTimeKind.Local).AddTicks(8232), "AQAAAAEAACcQAAAAEKFe0UIClDSR8dft1ldmi36e78ceUmDi7wX00/AioNvaSXriR48629i9o74RWgUbFA==", "4acd315e-39a2-49cb-b22f-d0082f99a67c" });
        }
    }
}
