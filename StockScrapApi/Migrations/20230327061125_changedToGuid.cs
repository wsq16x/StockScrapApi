using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class changedToGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScripCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "securities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecurityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecurityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScripCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_securities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "basicInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizedCapital = table.Column<double>(type: "float", nullable: true),
                    DebutTradingDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidUpCapital = table.Column<double>(type: "float", nullable: true),
                    FaceParValue = table.Column<double>(type: "float", nullable: true),
                    TotalOutstandingSecurity = table.Column<double>(type: "float", nullable: true),
                    InstrumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketLot = table.Column<int>(type: "int", nullable: true),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basicInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_basicInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "companyAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddrHeadOffice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddrFactory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecretaryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecretaryPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecretaryMobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecretaryEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_companyAddresses_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marketInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastTradingPrice = table.Column<double>(type: "float", nullable: true),
                    ClosingPrice = table.Column<double>(type: "float", nullable: true),
                    OpeningPrice = table.Column<double>(type: "float", nullable: true),
                    OpeningPriceAdjusted = table.Column<double>(type: "float", nullable: true),
                    ClosingPriceYesterday = table.Column<double>(type: "float", nullable: true),
                    DaysValue = table.Column<double>(type: "float", nullable: true),
                    DaysRangeMin = table.Column<double>(type: "float", nullable: true),
                    DaysRangeMax = table.Column<double>(type: "float", nullable: true),
                    Weaks52MovingRangeMin = table.Column<double>(type: "float", nullable: true),
                    Weaks52MovingRangeMax = table.Column<double>(type: "float", nullable: true),
                    Change = table.Column<double>(type: "float", nullable: true),
                    ChangePerct = table.Column<double>(type: "float", nullable: true),
                    DaysVolume = table.Column<double>(type: "float", nullable: true),
                    DaysTrade = table.Column<int>(type: "int", nullable: true),
                    MarketCapitalization = table.Column<double>(type: "float", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "otherInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ListingYear = table.Column<int>(type: "int", nullable: true),
                    MarketCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectronicShare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_otherInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_persons_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scrapeInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scrapeInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scrapeInfos_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shareHoldingPercts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SponsorDirector = table.Column<double>(type: "float", nullable: true),
                    Govt = table.Column<double>(type: "float", nullable: true),
                    Institute = table.Column<double>(type: "float", nullable: true),
                    Foreign = table.Column<double>(type: "float", nullable: true),
                    Public = table.Column<double>(type: "float", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shareHoldingPercts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shareHoldingPercts_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_basicInfo_CompanyId",
                table: "basicInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_companyAddresses_CompanyId",
                table: "companyAddresses",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_marketInfo_CompanyId",
                table: "marketInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_otherInfo_CompanyId",
                table: "otherInfo",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_persons_CompanyId",
                table: "persons",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_scrapeInfos_CompanyId",
                table: "scrapeInfos",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_shareHoldingPercts_CompanyId",
                table: "shareHoldingPercts",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basicInfo");

            migrationBuilder.DropTable(
                name: "companyAddresses");

            migrationBuilder.DropTable(
                name: "marketInfo");

            migrationBuilder.DropTable(
                name: "otherInfo");

            migrationBuilder.DropTable(
                name: "persons");

            migrationBuilder.DropTable(
                name: "scrapeInfos");

            migrationBuilder.DropTable(
                name: "securities");

            migrationBuilder.DropTable(
                name: "shareHoldingPercts");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
