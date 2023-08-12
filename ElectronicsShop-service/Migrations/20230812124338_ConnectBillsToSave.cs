using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class ConnectBillsToSave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "Bills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "6d49307a-6fe8-4a4b-9964-987ba64f1577");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "3d97295e-806e-477f-9152-d6520a5b7ff5");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2dc1c8d1-43d0-4c61-90f7-353a287b9c15", new DateTime(2023, 8, 12, 14, 43, 38, 45, DateTimeKind.Local).AddTicks(1364), "AQAAAAEAACcQAAAAELA+F8kA724HNYDv1nOIp15tYAcB1NGBcNHU2w1KljY9ouimHbHWs9FUzZViJMx/pw==", "f0aaba11-dbcf-43d0-808f-4d10e01912db" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "Bills");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "18e1e86d-2277-44b9-aaa2-92f53653fae7");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "171d45dc-eb0a-414c-8b20-0a172cd001ad");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e7c171b-5ce0-412e-84cc-f2768077c4aa", new DateTime(2023, 8, 11, 15, 48, 14, 563, DateTimeKind.Local).AddTicks(6418), "AQAAAAEAACcQAAAAENImlje3AjuwtrtlMUtP+HYioEtjZa98yBlXtX8zyFhEsoekiTkUFUu0qWzCIq8w/w==", "6be07e99-c0c3-46bc-a25c-54543cfd078f" });
        }
    }
}
