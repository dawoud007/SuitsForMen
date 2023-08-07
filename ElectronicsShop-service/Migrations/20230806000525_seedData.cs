using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectronicsShop_service.Migrations
{
    public partial class seedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "CreationDate", "NormalizedUserName", "Password", "PasswordHash", "SecurityStamp", "UserName", "WhatToSee" },
                values: new object[] { "a94789c1-3344-46ec-8acc-9f61d6e593b0", new DateTime(2023, 8, 6, 2, 5, 25, 606, DateTimeKind.Local).AddTicks(1028), "REDA", "Reda12@", "AQAAAAEAACcQAAAAENeP9RkM6XsIcr8XX7WaofWnXltSm8rTNzyYYUarI1wjbJPtAG3vSxF5xtce3/b+aA==", "4ac904c5-25da-40ef-ac88-91c155b864ac", "reda", "shop1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "fbb7709f-afd4-41a3-987b-0e0aa4f6f488");

            migrationBuilder.UpdateData(
                table: "ApplicationRole",
                keyColumn: "Id",
                keyValue: "2",
                column: "ConcurrencyStamp",
                value: "fcd79e82-08ca-4149-a109-1d663a70c2f7");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ConcurrencyStamp", "CreationDate", "NormalizedUserName", "Password", "PasswordHash", "SecurityStamp", "UserName", "WhatToSee" },
                values: new object[] { "e5c5c10f-6f94-426f-bcb1-73261cf3ee32", new DateTime(2023, 8, 6, 2, 1, 36, 205, DateTimeKind.Local).AddTicks(2058), "ADMIN@EXAMPLE.COM", "adminpassword123", "AQAAAAEAACcQAAAAEIcAQ4G4Te0B1rAYj5eKZxMHP+KpCcDKAKq8sHsJQVS70DAXn+q2SPQ+1HRT9GsSIA==", "f1ec185e-0dc6-4127-9b57-6689ba60307d", "admin@example.com", "Products" });
        }
    }
}
