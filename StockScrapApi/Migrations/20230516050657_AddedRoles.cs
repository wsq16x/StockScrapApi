using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
