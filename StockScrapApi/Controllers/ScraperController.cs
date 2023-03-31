using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Helpers;
using StockScrapApi.Scraper;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScraperController : ControllerBase
    {
        private readonly IScraper _scraper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMapFirebaseData _mapFirebaseData;

        public ScraperController(IScraper scraper, IBackgroundJobClient backgroundJobClient, IMapFirebaseData mapFirebaseData)
        {
            _scraper = scraper;
            _backgroundJobClient = backgroundJobClient;
            _mapFirebaseData = mapFirebaseData;
        }

        [HttpPost]
        public async Task<IActionResult> RunScraper()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _scraper.ScrapeAndPush(false, true));
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }

        [Route("/FetchCompanyLogos")]
        [HttpPost]
        public async Task<IActionResult> FetchCompanyLogos()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetCompanyLogo());
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }

        [Route("/FetchPersonPhotos")]
        [HttpPost]
        public async Task<IActionResult> FetchPersonPhotos()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetProfilePictures());
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }
    }
}
