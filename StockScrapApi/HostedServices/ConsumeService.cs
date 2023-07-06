using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.HostedServices
{
    public class ConsumeService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<ConsumeService> _logger;

        public ConsumeService(IServiceProvider services, ILogger<ConsumeService> logger)
        {
            _services = services;
            _logger = logger;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await FetchFirebaseData(stoppingToken);
            
            //disabled for testing purpose!
            //await EnqueueJob(stoppingToken);
        }

        private async Task FetchFirebaseData(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.FetchFirebaseData(stoppingToken);
            }
        }

        private async Task EnqueueJob(CancellationToken stoppingToken)
        {
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.EnqueueJob(stoppingToken);
            }
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            

            await base.StopAsync(stoppingToken);
        }
    }
}
