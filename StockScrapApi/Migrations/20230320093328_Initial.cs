using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockScrapApi.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScripCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "basicInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizedCapital = table.Column<float>(type: "real", nullable: true),
                    DebutTradingDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidUpCapital = table.Column<float>(type: "real", nullable: true),
                    FaceParValue = table.Column<float>(type: "real", nullable: true),
                    TotalOutstandingSecurity = table.Column<float>(type: "real", nullable: true),
                    InstrumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketLot = table.Column<int>(type: "int", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basicInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_basicInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "companyAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companyAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_companyAddresses_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "marketInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastTradingPrice = table.Column<float>(type: "real", nullable: true),
                    ClosingPrice = table.Column<float>(type: "real", nullable: true),
                    OpeningPrice = table.Column<float>(type: "real", nullable: true),
                    OpeningPriceAdjusted = table.Column<float>(type: "real", nullable: true),
                    ClosingPriceYesterday = table.Column<float>(type: "real", nullable: true),
                    DaysValue = table.Column<float>(type: "real", nullable: true),
                    DaysRangeMin = table.Column<float>(type: "real", nullable: true),
                    DaysRangeMax = table.Column<float>(type: "real", nullable: true),
                    Weaks52MovingRangeMin = table.Column<float>(type: "real", nullable: true),
                    Weaks52MovingRangeMax = table.Column<float>(type: "real", nullable: true),
                    change = table.Column<float>(type: "real", nullable: true),
                    changePerct = table.Column<float>(type: "real", nullable: true),
                    DaysVolume = table.Column<float>(type: "real", nullable: true),
                    DaysTrade = table.Column<int>(type: "int", nullable: true),
                    MarketCapitalization = table.Column<float>(type: "real", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_marketInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "otherInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingYear = table.Column<int>(type: "int", nullable: false),
                    MarketCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElectronicShare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_otherInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scrapeInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scrapeInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scrapeInfo_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shareHoldingPercts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorDirector = table.Column<float>(type: "real", nullable: true),
                    Govt = table.Column<float>(type: "real", nullable: true),
                    Institute = table.Column<float>(type: "real", nullable: true),
                    Foreign = table.Column<float>(type: "real", nullable: true),
                    Public = table.Column<float>(type: "real", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shareHoldingPercts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shareHoldingPercts_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_basicInfo_CompanyId",
                table: "basicInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_companyAddresses_CompanyId",
                table: "companyAddresses",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_marketInfo_CompanyId",
                table: "marketInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_otherInfo_CompanyId",
                table: "otherInfo",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_scrapeInfo_CompanyId",
                table: "scrapeInfo",
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
                name: "scrapeInfo");

            migrationBuilder.DropTable(
                name: "shareHoldingPercts");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
