using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class FirebaseData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "persons",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "persons",
                newName: "Image");
        }
    }
}
