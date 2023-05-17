using Microsoft.AspNetCore.Identity;
using StockScrapApi.Models;

namespace StockScrapApi.Configuration
{
    public static class AppExtension
    {
        public static async Task ConfigureSuperUser(this WebApplication app)
        {
            var userName = Environment.GetEnvironmentVariable("STOCK_SUPERUSER");
            var passWord = Environment.GetEnvironmentVariable("STOCK_SUPERPASSWORD");
            if(userName == null || passWord == null)
            {
                throw new Exception("Environemnt variables not set!");
            }
            var roles = new List<string>{ "Administrator", "SuperUser", "Employee"};

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

                // Check if the default user exists
                var defaultUser = await userManager.FindByNameAsync(userName);
                if (defaultUser == null)
                {
                    // Create a new instance of IdentityUser
                    var user = new ApiUser
                    {
                        UserName = userName,
                        Email = userName
                    };

                    // Create the default user
                    var result = await userManager.CreateAsync(user, passWord);

                    if (result.Succeeded)
                    {
                        try
                        {
                           await userManager.AddToRolesAsync(user, roles);
                        }
                        catch (Exception ex)
                        {
                            await userManager.DeleteAsync(user);
                            throw new Exception("failed to add roles!");
                        }

                    }
                    else
                    {
                        throw new Exception("Failed to create SuperUser");
                    }
                    
                }
            }
        }
    }
}
