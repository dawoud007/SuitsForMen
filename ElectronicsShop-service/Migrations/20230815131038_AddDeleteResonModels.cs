using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class AddDeleteResonModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("10f456d7-d758-45fc-91ed-742e160419bf"));

            migrationBuilder.CreateTable(
                name: "DeleteReasons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeleteReasonDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeleteData = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteReasons", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("a135e70f-f393-4dbb-bb20-bf5d1de95123"), new DateTime(2023, 8, 15, 15, 10, 37, 791, DateTimeKind.Local).AddTicks(5368), "Reda12@", "Admin", "reda", "shop1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeleteReasons");

            migrationBuilder.DeleteData(
                table: "Shops",
                keyColumn: "Id",
                keyValue: new Guid("a135e70f-f393-4dbb-bb20-bf5d1de95123"));

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "CreationDate", "Password", "Role", "UserName", "WhatToSee" },
                values: new object[] { new Guid("10f456d7-d758-45fc-91ed-742e160419bf"), new DateTime(2023, 8, 15, 14, 45, 3, 364, DateTimeKind.Local).AddTicks(8707), "Reda12@", "Admin", "reda", "shop1" });
        }
    }
}
