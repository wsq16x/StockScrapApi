using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class Transfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88169827-93ca-48a9-9ba4-f8ae98597181");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b48b9631-a27b-4baf-aff5-aac88efbb764");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb7f520d-4bcd-409a-a32b-2bf45d250428");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5fd7d106-555d-4102-9e26-429bafd2c69e", "b4481f66-1a8f-4bee-bee7-17120b3f630b", "SuperUser", "SUPERUSER" },
                    { "80f94f0c-6396-4c1b-80a4-bd3e9ba63692", "61300969-4d52-4a7f-acf1-b34255b6be21", "Employee", "EMPLOYEE" },
                    { "95507621-a0ed-4750-a89b-bff98d17ddf0", "524bdf91-61ef-4ef0-9554-4dc3c12d1bcc", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5fd7d106-555d-4102-9e26-429bafd2c69e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "80f94f0c-6396-4c1b-80a4-bd3e9ba63692");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95507621-a0ed-4750-a89b-bff98d17ddf0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88169827-93ca-48a9-9ba4-f8ae98597181", "4aedf59c-5082-4abf-bf4e-b19e4063e89e", "Administrator", "ADMINISTRATOR" },
                    { "b48b9631-a27b-4baf-aff5-aac88efbb764", "9ec822a7-6b2e-4abf-bb60-edd317f830eb", "SuperUser", "SUPERUSER" },
                    { "fb7f520d-4bcd-409a-a32b-2bf45d250428", "e3ee31ed-5d59-4f98-a343-890c66a7101d", "Employee", "EMPLOYEE" }
                });
        }
    }
}
