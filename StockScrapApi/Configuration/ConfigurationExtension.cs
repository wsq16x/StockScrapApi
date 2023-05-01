using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Serilog.Events;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Configuration
{
    public static class ConfigurationExtension
    {
        public static void ConfigureSeriLog(this IServiceCollection services, WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Seq("http://localhost:5341")
            .WriteTo.File(path: Path.Combine(builder.Environment.ContentRootPath, "SeriLog", "Logs", "log-.txt"),
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss}[{Level:u3}]{Message:lj}{NewLine}{Exception}",
                  rollingInterval: RollingInterval.Day,
                  retainedFileCountLimit: 10,
                  restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();
        }

        //custom service to handle exceptions
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Log.Error($"Something went wrong in the {contextFeature.Error}");
                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Please Try Again Later."
                        }.ToString());
                    }
                });
            });
        }
        public static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(opt => opt
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(30),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

            services.AddHangfireServer();

        }
    }

}
