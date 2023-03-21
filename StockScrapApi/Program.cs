using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;
using StockScrapApi.Configuration;
using StockScrapApi.Data;
using StockScrapApi.Hangfire;
using StockScrapApi.Profiles;
using StockScrapApi.Scraper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSeriLog();

//Db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"))
    );

//hangfire
builder.Services.ConfigureHangfire(builder.Configuration);

builder.Services.AddAutoMapper(typeof(MapperProfile));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IScrapeData, ScrapeData>();
builder.Services.AddTransient<IScraper, Scraper>();
//builder.Services.AddTransient<IHangfireJob, HangfireJob>();

//This is requird for .NET 6
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(opt => opt.MapHangfireDashboard("/hangfire"));
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
