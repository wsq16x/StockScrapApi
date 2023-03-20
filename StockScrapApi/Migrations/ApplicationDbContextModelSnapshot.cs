﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockScrapApi.Data;

#nullable disable

namespace StockScrapApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StockScrapApi.Model.ScrapeInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("scrapeInfos");
                });

            modelBuilder.Entity("StockScrapApi.Models.BasicInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("AuthorizedCapital")
                        .HasColumnType("real");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("DebutTradingDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("FaceParValue")
                        .HasColumnType("real");

                    b.Property<string>("InstrumentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MarketLot")
                        .HasColumnType("int");

                    b.Property<float?>("PaidUpCapital")
                        .HasColumnType("real");

                    b.Property<string>("Sector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<float?>("TotalOutstandingSecurity")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("basicInfo");
                });

            modelBuilder.Entity("StockScrapApi.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScripCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("companies");
                });

            modelBuilder.Entity("StockScrapApi.Models.CompanyAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AddrFactory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AddrHeadOffice")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretaryEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretaryMobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretaryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecretaryPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("companyAddresses");
                });

            modelBuilder.Entity("StockScrapApi.Models.MarketInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float?>("ClosingPrice")
                        .HasColumnType("real");

                    b.Property<float?>("ClosingPriceYesterday")
                        .HasColumnType("real");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<float?>("DaysRangeMax")
                        .HasColumnType("real");

                    b.Property<float?>("DaysRangeMin")
                        .HasColumnType("real");

                    b.Property<int?>("DaysTrade")
                        .HasColumnType("int");

                    b.Property<float?>("DaysValue")
                        .HasColumnType("real");

                    b.Property<float?>("DaysVolume")
                        .HasColumnType("real");

                    b.Property<float?>("LastTradingPrice")
                        .HasColumnType("real");

                    b.Property<float?>("MarketCapitalization")
                        .HasColumnType("real");

                    b.Property<float?>("OpeningPrice")
                        .HasColumnType("real");

                    b.Property<float?>("OpeningPriceAdjusted")
                        .HasColumnType("real");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<float?>("Weaks52MovingRangeMax")
                        .HasColumnType("real");

                    b.Property<float?>("Weaks52MovingRangeMin")
                        .HasColumnType("real");

                    b.Property<float?>("change")
                        .HasColumnType("real");

                    b.Property<float?>("changePerct")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("marketInfo");
                });

            modelBuilder.Entity("StockScrapApi.Models.OtherInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ElectronicShare")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ListingYear")
                        .HasColumnType("int");

                    b.Property<string>("MarketCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("otherInfo");
                });

            modelBuilder.Entity("StockScrapApi.Models.ShareHoldingPerct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<float?>("Foreign")
                        .HasColumnType("real");

                    b.Property<float?>("Govt")
                        .HasColumnType("real");

                    b.Property<float?>("Institute")
                        .HasColumnType("real");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Public")
                        .HasColumnType("real");

                    b.Property<float?>("SponsorDirector")
                        .HasColumnType("real");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("shareHoldingPercts");
                });

            modelBuilder.Entity("StockScrapApi.Model.ScrapeInfo", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockScrapApi.Models.BasicInfo", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockScrapApi.Models.CompanyAddress", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockScrapApi.Models.MarketInfo", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockScrapApi.Models.OtherInfo", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("StockScrapApi.Models.ShareHoldingPerct", b =>
                {
                    b.HasOne("StockScrapApi.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });
#pragma warning restore 612, 618
        }
    }
}
