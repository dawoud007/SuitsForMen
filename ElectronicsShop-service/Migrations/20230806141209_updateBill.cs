using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class updateBill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Bills",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Bills");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "bd11b051-8d58-40da-8243-6aae4c51bd29");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "c2d2fe5f-ba8b-4284-a57c-c06cee96a1f1");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a94789c1-3344-46ec-8acc-9f61d6e593b0", new DateTime(2023, 8, 6, 2, 5, 25, 606, DateTimeKind.Local).AddTicks(1028), "AQAAAAEAACcQAAAAENeP9RkM6XsIcr8XX7WaofWnXltSm8rTNzyYYUarI1wjbJPtAG3vSxF5xtce3/b+aA==", "4ac904c5-25da-40ef-ac88-91c155b864ac" });
        }
    }
}
