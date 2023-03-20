using Microsoft.EntityFrameworkCore;
using StockScrapApi.Model;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Company> companies { get; set; }
        public DbSet<BasicInfo> basicInfo { get; set; }
        public DbSet<CompanyAddress>  companyAddresses { get; set; }
        public DbSet<MarketInfo> marketInfo { get; set; }
        public DbSet<OtherInfo> otherInfo { get; set; }
        public DbSet<ShareHoldingPerct> shareHoldingPercts { get; set; }
        public DbSet<ScrapeInfo> scrapeInfos { get; set; }
    }
}
