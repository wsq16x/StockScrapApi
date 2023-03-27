using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.HostedServices
{
    public interface IScopedProcessingService
    {
        Task EnqueueJob(CancellationToken stoppingToken);
    }
}
