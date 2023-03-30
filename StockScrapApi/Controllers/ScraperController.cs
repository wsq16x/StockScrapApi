using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Scraper;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        private readonly IScraper _scraper;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ScraperController(IScraper scraper, IBackgroundJobClient backgroundJobClient)
        {
            _scraper = scraper;
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpPost]
        public async Task<IActionResult> RunScraper()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _scraper.ScrapeAndPush(false, true));
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }
    }
}
