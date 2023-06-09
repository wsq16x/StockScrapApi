﻿using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Scraper
{
    public interface IScraper
    {
        [AutomaticRetry(Attempts = 5)]
        Task ScrapeAndPush(bool? automaticRetry=false, bool? forceScrape=false);
    }
}
