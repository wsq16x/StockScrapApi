using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class dsex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Dse30Shares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ltp = table.Column<double>(type: "float", nullable: true),
                    High = table.Column<double>(type: "float", nullable: true),
                    Low = table.Column<double>(type: "float", nullable: true),
                    Closep = table.Column<double>(type: "float", nullable: true),
                    Ycp = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    Trade = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    Volume = table.Column<long>(type: "bigint", nullable: true),
                    InfoTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dse30Shares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DsexShares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TradingCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ltp = table.Column<double>(type: "float", nullable: true),
                    High = table.Column<double>(type: "float", nullable: true),
                    Low = table.Column<double>(type: "float", nullable: true),
                    Closep = table.Column<double>(type: "float", nullable: true),
                    Ycp = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    Trade = table.Column<long>(type: "bigint", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: true),
                    Volume = table.Column<long>(type: "bigint", nullable: true),
                    InfoTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DsexShares", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d36c3e9-e44a-45b2-a784-923dc849ee16", "6ea8e956-49eb-4660-9918-afe985759ece", "SuperUser", "SUPERUSER" },
                    { "24181c0e-5feb-47f0-a251-c9a803ce439b", "3c57e246-589e-41ee-b71e-0245a6c88c44", "Employee", "EMPLOYEE" },
                    { "59389a2a-eeba-40fb-a490-c8624a335e4e", "f563db54-653d-41b9-bbd2-7113197b9d03", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dse30Shares");

            migrationBuilder.DropTable(
                name: "DsexShares");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d36c3e9-e44a-45b2-a784-923dc849ee16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24181c0e-5feb-47f0-a251-c9a803ce439b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59389a2a-eeba-40fb-a490-c8624a335e4e");

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
    }
}
