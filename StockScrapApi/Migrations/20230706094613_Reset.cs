using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f4122fc-c03d-49b0-ac83-1ddca16726a9", "00c64f15-5253-4a50-9e6e-bb7c1df652a5", "Employee", "EMPLOYEE" },
                    { "cd3a1c28-d357-47ae-9a3c-56da5147d478", "bb961aeb-5818-4be3-bc3d-5db3d3c6db42", "Administrator", "ADMINISTRATOR" },
                    { "f7fe3162-f321-48e4-bedd-49279eef50bc", "3905c1f0-ca56-4e33-8410-f3b1609d2cc5", "SuperUser", "SUPERUSER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f4122fc-c03d-49b0-ac83-1ddca16726a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd3a1c28-d357-47ae-9a3c-56da5147d478");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f7fe3162-f321-48e4-bedd-49279eef50bc");

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
    }
}
