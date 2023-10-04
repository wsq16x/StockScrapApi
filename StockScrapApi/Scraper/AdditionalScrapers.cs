using HtmlAgilityPack;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using StockScrapApi.Data;
using StockScrapApi.Models;
using System;
using System.Text.RegularExpressions;

namespace StockScrapApi.Scraper
{
    public class AdditionalScrapers : IAdditionalScrapers
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdditionalScrapers> _logger;

        public AdditionalScrapers(ApplicationDbContext context, ILogger<AdditionalScrapers> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task DsexScraper()
        {
            var url1 = "https://www.dsebd.org/dseX_share.php";

            try
            {
                var web = new HtmlWeb();
                var page = web.Load(url1);

                if (web.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError("Failed to load page. Status Code {StatusCode}", web.StatusCode);
                    return;
                }

                var titleNode = page.DocumentNode.SelectSingleNode(string.Format("//h2[@class='BodyHead topBodyHead']"));
                var title = titleNode.InnerText;

                //recognize date-time with the power of NLP muahahaha!
                var recognizer = DateTimeRecognizer.RecognizeDateTime(title, Culture.English);
                var keyValuePairs = (List<Dictionary<string, string>>?)recognizer.First()?.Resolution["values"];
                var dateTime = keyValuePairs?.First().GetValueOrDefault("value");

                if (dateTime == null)
                {
                    _logger.LogError("Failed to parse datetime!");
                    return;
                }

                _logger.LogInformation("Scraping Dsex Table for {dateTime}", dateTime);

                var data = new List<Dsex>();

                //get the table!

                var shares = page.DocumentNode.SelectNodes(string.Format("//table[contains(@class, 'shares-table')]/tr"));

                for (int i = 1; i < shares.Count; i++)
                {
                    long longResult;
                    double doubleResult;
                    DateTime dateTimeResult;

                    var dsex = new Dsex();

                    var row = shares[i];

                    var cols = shares[i].SelectNodes("./td");

                    dsex.TradingCode = cols[1].InnerText.Trim();

                    if (double.TryParse(cols[2].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Ltp = doubleResult;
                    if (double.TryParse(cols[3].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.High = doubleResult;
                    if (double.TryParse(cols[4].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Low = doubleResult;
                    if (double.TryParse(cols[5].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Closep = doubleResult;
                    if (double.TryParse(cols[6].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Ycp = doubleResult;
                    if (double.TryParse(cols[7].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Change = doubleResult;
                    if (long.TryParse(cols[8].InnerText.Trim().Replace(",", ""), out longResult))
                        dsex.Trade = longResult;
                    if (double.TryParse(cols[9].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Value = doubleResult;
                    if (long.TryParse(cols[10].InnerText.Trim().Replace(",", ""), out longResult))
                        dsex.Volume = longResult;

                    if (DateTime.TryParse(dateTime, out dateTimeResult))
                        dsex.InfoTime = dateTimeResult;

                    dsex.TimeStamp = DateTime.Now;
                    data.Add(dsex);
                }

                await _context.AddRangeAsync(data);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Done scraping Dsex Table for {dateTime}", dateTime);
            }
            catch (HtmlWebException webEx)
            {
                _logger.LogError(webEx, "Error occured scraping Dsex table!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured scraping Dsex table!");
            }
        }

        public async Task Dse30Scraper()
        {
            var url2 = "https://www.dsebd.org/dse30_share.php"; ;

            try
            {
                var web = new HtmlWeb();
                var page = web.Load(url2);

                if (web.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError("Failed to load page. Status Code {StatusCode}", web.StatusCode);
                    return;
                }

                var titleNode = page.DocumentNode.SelectSingleNode(string.Format("//h2[@class='BodyHead topBodyHead']"));
                var title = titleNode.InnerText;
                //recognize date-time with the power of NLP muahahaha!
                var recognizer = DateTimeRecognizer.RecognizeDateTime(title, Culture.English);
                var keyValuePairs = (List<Dictionary<string, string>>?)recognizer.First()?.Resolution["values"];
                var dateTime = keyValuePairs?.First().GetValueOrDefault("value");

                if (dateTime == null)
                {
                    _logger.LogError("Failed to parse datetime!");
                    return;
                }

                var data = new List<Dse30>();

                //get the table!

                var shares = page.DocumentNode.SelectNodes(string.Format("//table[contains(@class, 'shares-table')]/tr"));

                for (int i = 1; i < shares.Count; i++)
                {
                    long longResult;
                    double doubleResult;
                    DateTime dateTimeResult;

                    var dsex = new Dse30();

                    var row = shares[i];

                    var cols = shares[i].SelectNodes("./td");

                    dsex.TradingCode = cols[1].InnerText.Trim();

                    if (double.TryParse(cols[2].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Ltp = doubleResult;
                    if (double.TryParse(cols[3].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.High = doubleResult;
                    if (double.TryParse(cols[4].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Low = doubleResult;
                    if (double.TryParse(cols[5].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Closep = doubleResult;
                    if (double.TryParse(cols[6].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Ycp = doubleResult;
                    if (double.TryParse(cols[7].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Change = doubleResult;
                    if (long.TryParse(cols[8].InnerText.Trim().Replace(",", ""), out longResult))
                        dsex.Trade = longResult;
                    if (double.TryParse(cols[9].InnerText.Trim().Replace(",", ""), out doubleResult))
                        dsex.Value = doubleResult;
                    if (long.TryParse(cols[10].InnerText.Trim().Replace(",", ""), out longResult))
                        dsex.Volume = longResult;

                    if (DateTime.TryParse(dateTime, out dateTimeResult))
                        dsex.InfoTime = dateTimeResult;

                    dsex.TimeStamp = DateTime.Now;
                    data.Add(dsex);
                }

                await _context.AddRangeAsync(data);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Done scraping Dse30 Table for {dateTime}", dateTime);
            }
            catch (HtmlWebException webEx)
            {
                _logger.LogError(webEx, "Error occured scraping Dse30 table!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured scraping Dse30 table!");
            }
        }
    }
}