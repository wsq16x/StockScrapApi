using Hangfire;
using StockScrapApi.Helpers;
using StockScrapApi.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.HostedServices
{
    public class ScopedProcessingService : IScopedProcessingService
    {
        private ILogger<ScopedProcessingService> _logger;
        private IScraper _scraper;
        private IBackgroundJobClient _backGroundJobClient;
        private IInitialize _initialize;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IScraper scraper, IBackgroundJobClient backgroundJobClient, IInitialize initialize)
        {
            _logger = logger;
            _scraper = scraper;
            _backGroundJobClient = backgroundJobClient;
            _initialize = initialize;
        }
        public Task EnqueueJob(CancellationToken stoppingToken)
        {

            //var jobId = _backGroundJobClient.Enqueue(() => _scraper.ScrapeAndPush());
            RecurringJob.AddOrUpdate("scrapeData", () => _scraper.ScrapeAndPush(), "*/40 * * * *");

            return Task.CompletedTask;
        }
        public async Task FetchFirebaseData(CancellationToken stoppingToken)
        {
            await _initialize.InitDatabase();

            return;
        }
    }
}
