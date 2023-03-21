using Hangfire;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Data;
using StockScrapApi.Hangfire;
using StockScrapApi.Scraper;

namespace StockScrapApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IScraper _scraper;
        private readonly ApplicationDbContext _context;
        private readonly IBackgroundJobClient _backGroundJobClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ApplicationDbContext context, IScraper scraper, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _scraper = scraper;
            _context = context;
            _backGroundJobClient = backgroundJobClient;

        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public ActionResult Scrape()
        {
            var jobId = _backGroundJobClient.Schedule(() => _scraper.ScrapeAndPush(), TimeSpan.FromMinutes(5));
            return Ok(string.Format("Job Created with {0}", jobId));
        }
    }
}