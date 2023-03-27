using Hangfire;
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

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IScraper scraper, IBackgroundJobClient backgroundJobClient)
        {
            _logger = logger;
            _scraper = scraper;
            _backGroundJobClient = backgroundJobClient;
        }
        public Task EnqueueJob(CancellationToken stoppingToken)
        {
            //var jobId = _backGroundJobClient.Enqueue(() => _scraper.ScrapeAndPush());
            RecurringJob.AddOrUpdate("scrapeData", () => _scraper.ScrapeAndPush(), "*/5 * * * *");

            return Task.CompletedTask;
        }
    }
}
