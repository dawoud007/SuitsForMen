using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class AddDeleteResonModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("65780a9c-6cb9-4eea-bb06-2a1b47ff08f4"));

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("10f456d7-d758-45fc-91ed-742e160419bf"), new DateTime(2023, 8, 15, 14, 45, 3, 364, DateTimeKind.Local).AddTicks(8707), "Reda12@", "Admin", "reda", "shop1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("10f456d7-d758-45fc-91ed-742e160419bf"));

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("65780a9c-6cb9-4eea-bb06-2a1b47ff08f4"), new DateTime(2023, 8, 14, 12, 17, 18, 296, DateTimeKind.Local).AddTicks(876), "Reda12@", "Admin", "reda", "shop1" });
        }
    }
}
