using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class AddAttriputesToReasonToDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a135e70f-f393-4dbb-bb20-bf5d1de95123"));

            migrationBuilder.AddColumn<int>(
                name: "amount",
                table: "DeleteReasons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "workerName",
                table: "DeleteReasons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("4ec6657b-bbe8-4576-9848-bfb8c70aeea0"), new DateTime(2023, 8, 15, 17, 52, 28, 297, DateTimeKind.Local).AddTicks(8798), "Reda12@", "Admin", "reda", "shop1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("4ec6657b-bbe8-4576-9848-bfb8c70aeea0"));

            migrationBuilder.DropColumn(
                name: "amount",
                table: "DeleteReasons");

            migrationBuilder.DropColumn(
                name: "workerName",
                table: "DeleteReasons");

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("a135e70f-f393-4dbb-bb20-bf5d1de95123"), new DateTime(2023, 8, 15, 15, 10, 37, 791, DateTimeKind.Local).AddTicks(5368), "Reda12@", "Admin", "reda", "shop1" });
        }
    }
}
