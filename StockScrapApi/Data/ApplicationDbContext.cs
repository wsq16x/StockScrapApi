using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StockScrapApi.Configuration;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApiUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Company> companies { get; set; }
        public DbSet<BasicInfo> basicInfo { get; set; }
        public DbSet<CompanyAddress>  companyAddresses { get; set; }
        public DbSet<MarketInfo> marketInfo { get; set; }
        public DbSet<OtherInfo> otherInfo { get; set; }
        public DbSet<ShareHoldingPerct> shareHoldingPercts { get; set; }
        public DbSet<ScrapeInfo> scrapeInfos { get; set; }
        public DbSet<Security> securities { get; set; }
        public DbSet<Person> persons { get; set; }
        public DbSet<CompanyLogo> companyLogos { get; set; }
        public DbSet<CompanyFirebase> companiesFirebase { get; set; }
        public DbSet<PersonFirebase> personsFirebase { get; set; }
        public DbSet<ProfilePicture> profilePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());

        }
    }
}
