using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "changePerct",
                table: "marketInfo",
                newName: "ChangePerct");

            migrationBuilder.RenameColumn(
                name: "change",
                table: "marketInfo",
                newName: "Change");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChangePerct",
                table: "marketInfo",
                newName: "changePerct");

            migrationBuilder.RenameColumn(
                name: "Change",
                table: "marketInfo",
                newName: "change");
        }
    }
}
