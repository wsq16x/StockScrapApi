using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class mod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_scrapeInfo_companies_CompanyId",
                table: "scrapeInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_scrapeInfo",
                table: "scrapeInfo");

            migrationBuilder.RenameTable(
                name: "scrapeInfo",
                newName: "scrapeInfos");

            migrationBuilder.RenameIndex(
                name: "IX_scrapeInfo_CompanyId",
                table: "scrapeInfos",
                newName: "IX_scrapeInfos_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "otherInfo",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_scrapeInfos",
                table: "scrapeInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_scrapeInfos_companies_CompanyId",
                table: "scrapeInfos",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_scrapeInfos_companies_CompanyId",
                table: "scrapeInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_scrapeInfos",
                table: "scrapeInfos");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "companies");

            migrationBuilder.RenameTable(
                name: "scrapeInfos",
                newName: "scrapeInfo");

            migrationBuilder.RenameIndex(
                name: "IX_scrapeInfos_CompanyId",
                table: "scrapeInfo",
                newName: "IX_scrapeInfo_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "otherInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_scrapeInfo",
                table: "scrapeInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_scrapeInfo_companies_CompanyId",
                table: "scrapeInfo",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
