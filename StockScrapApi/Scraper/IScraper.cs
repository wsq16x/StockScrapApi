using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Scraper
{
    public interface IScraper
    {
        void ScrapeAndPush();
    }
}
