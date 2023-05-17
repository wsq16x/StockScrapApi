using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Serilog;
using StockScrapApi.Configuration;
using StockScrapApi.Data;
using StockScrapApi.Helpers;
using StockScrapApi.HostedServices;
using StockScrapApi.Profiles;
using StockScrapApi.Scraper;
using Newtonsoft.Json;
using StockScrapApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSeriLog(builder);

//Db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
    );

//hangfire
builder.Services.ConfigureHangfire(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MapperProfile));
// Add services to the container.

//Auth and JWT
//Add Identity
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthorization();

builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddControllers().AddNewtonsoftJson(
                 op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IScrapeData, ScrapeData>();
builder.Services.AddTransient<IScraper, Scraper>();
builder.Services.AddTransient<IGetFirebaseData, GetFirebaseData>();
builder.Services.AddTransient<IMapFirebaseData, MapFirebaseData>();
builder.Services.AddTransient<IInitialize, Initialize>();
builder.Services.AddHostedService<ConsumeService>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
//builder.Services.AddTransient<IHangfireJob, HangfireJob>();

//add CORS and configure CORS
builder.Services.AddCors(f =>
{
    f.AddPolicy("AllowAll", f =>
    f.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

//This is requird for .NET 6
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "StockApi");
});

app.ConfigureExceptionHandler();
app.UseHangfireDashboard();
app.UseRouting();
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(opt => opt.MapHangfireDashboard("/hangfire"));



app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(builder.Environment.ContentRootPath, "Files", "Images")),
    RequestPath = "/Images"
});
app.MapControllers();


try
{
    Log.Information("Application Is Starting....");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed to start!");
}
finally
{
    Log.CloseAndFlush();
}