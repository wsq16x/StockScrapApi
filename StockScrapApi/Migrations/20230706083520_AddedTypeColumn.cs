using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "companies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4a04fe9d-e0d5-424f-8d53-eb89f40417fe", "520c3b7e-8c68-4959-a0c8-156be157a717", "SuperUser", "SUPERUSER" },
                    { "7e8a776b-9bb6-443d-85bd-4cc916b8836e", "34788ad2-fb5b-4480-a17e-1a49c728b9a0", "Administrator", "ADMINISTRATOR" },
                    { "acf17cfa-9e95-49c1-819a-2fa53152e193", "e0c8dd3e-918b-4fb5-ae3f-1b8cbdd8b3c3", "Employee", "EMPLOYEE" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4a04fe9d-e0d5-424f-8d53-eb89f40417fe");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e8a776b-9bb6-443d-85bd-4cc916b8836e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "acf17cfa-9e95-49c1-819a-2fa53152e193");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "companies");

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
    }
}
