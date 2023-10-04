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
        private IAdditionalScrapers _additionalScrapers;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger, IScraper scraper, IBackgroundJobClient backgroundJobClient, IInitialize initialize, IAdditionalScrapers additionalScrapers)
        {
            _logger = logger;
            _scraper = scraper;
            _backGroundJobClient = backgroundJobClient;
            _initialize = initialize;
            _additionalScrapers = additionalScrapers;
        }

        public async Task EnqueueJob(CancellationToken stoppingToken)
        {
            //var jobId = _backGroundJobClient.Enqueue(() => _scraper.ScrapeAndPush());
            RecurringJob.AddOrUpdate("scrapeData", () => _scraper.ScrapeAndPush(true, false), "0 12 * * *");
            RecurringJob.AddOrUpdate("scrapeDsex", () => _additionalScrapers.DsexScraper(), "*/10 * * * *");
            RecurringJob.AddOrUpdate("scrapeDse30", () => _additionalScrapers.Dse30Scraper(), "*/10 * * * *");
        }

        public async Task FetchFirebaseData(CancellationToken stoppingToken)
        {
            await _initialize.InitDatabase();

            return;
        }
    }
}