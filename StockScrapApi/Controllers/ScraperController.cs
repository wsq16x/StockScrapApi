using Hangfire;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "SuperUser")]
        [HttpPost]
        public async Task<IActionResult> RunScraper()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _scraper.ScrapeAndPush(false, false));
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }

        [Authorize(Roles = "SuperUser")]
        [Route("/FetchCompanyLogos")]
        [HttpPost]
        public async Task<IActionResult> FetchCompanyLogos()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetCompanyLogo());
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }

        [Authorize(Roles = "SuperUser")]
        [Route("/FetchPersonPhotos")]
        [HttpPost]
        public async Task<IActionResult> FetchPersonPhotos()
        {
            var JobId = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetProfilePictures());
            return Ok(string.Format("Job Created with Id {0}", JobId));
        }

        //[Authorize(Roles = "SuperUser")]
        //[Route("Test")]
        //[HttpPost]
        //public async Task<IActionResult> TestScraper()
        //{
        //    await _scraper.ScrapeAndPush();
        //    return Ok();
        //}
    }
}