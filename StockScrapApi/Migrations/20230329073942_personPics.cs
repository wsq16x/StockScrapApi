using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class personPics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_companies_CompanyId",
                table: "persons");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "persons",
                newName: "ImageUrl");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "persons",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "FirebaseCompId",
                table: "persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "profilePictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profilePictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_profilePictures_persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_profilePictures_PersonId",
                table: "profilePictures",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_persons_companies_CompanyId",
                table: "persons",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_persons_companies_CompanyId",
                table: "persons");

            migrationBuilder.DropTable(
                name: "profilePictures");

            migrationBuilder.DropColumn(
                name: "FirebaseCompId",
                table: "persons");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "persons",
                newName: "ImageId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "persons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_persons_companies_CompanyId",
                table: "persons",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
