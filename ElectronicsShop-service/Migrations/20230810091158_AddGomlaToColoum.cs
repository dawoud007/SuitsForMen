using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class AddGomlaToColoum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Clothes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Gomla",
                table: "Clothes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "limit",
                table: "Clothes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c519107e-1de2-420e-8ce3-fab3fd7b3acd");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "8870fce8-9a2e-4b4c-aed4-8471d880b4d3");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b415773-72eb-40b3-8c50-210c6b2afa95", new DateTime(2023, 8, 10, 11, 11, 58, 112, DateTimeKind.Local).AddTicks(5257), "AQAAAAEAACcQAAAAEIlEQPw2bI1zpqhP1QEYHD4slvNoIVRw1kFJ2zuK58bx7FgZHJezLPx/O2P0GK2gnQ==", "21ee4aca-7521-4d13-9835-1733a8962d87" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gomla",
                table: "Clothes");

            migrationBuilder.DropColumn(
                name: "limit",
                table: "Clothes");

            migrationBuilder.AlterColumn<string>(
                name: "StoreName",
                table: "Clothes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
