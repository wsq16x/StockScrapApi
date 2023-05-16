using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StockScrapApi.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                    new IdentityRole
                    {
                        Name = "SuperUser",
                        NormalizedName = "SUPERUSER",
                    },
                    new IdentityRole
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR",
                    },
                    new IdentityRole
                    {
                        Name = "Employee",
                        NormalizedName = "EMPLOYEE"
                    }
                );
        }
    }
}
