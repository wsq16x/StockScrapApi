using HtmlAgilityPack;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using StockScrapApi.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace StockScrapApi.Scraper
{
    public class ScrapeData : IScrapeData
    {
        private readonly ILogger<ScrapeData> _logger;

        public ScrapeData(ILogger<ScrapeData> logger)
        {
            _logger = logger;
        }
        public TableWithHead GetTables( string rootUrl, string url)
        {
            var DocUrl = String.Format("{0}/{1}", rootUrl, url);
            HtmlWeb page = new HtmlWeb();
            var target = page.Load(DocUrl);
            var xpath = String.Format("//*[@id='company']");

            //this will be looped for all
            var headers = target.DocumentNode.Descendants().Where(c => c.HasClass("topBodyHead") && c.HasClass("BodyHead")).First().InnerText;
            var nodes = target.DocumentNode.SelectNodes(xpath);

            //foreach (var node in nodes)
            //{
            //    Console.WriteLine(node.OuterHtml);
            //    Console.WriteLine("----------------------------------------");
            //}
            return new TableWithHead { Head = headers, Nodes = nodes };
        }
        public BasicInfo GetBasicInfo(TableWithHead allTables)
        {
            double valF;
            int valI;

            var authorizedCapital = allTables.Nodes[2].SelectSingleNode("./tr[1]/td[1]").InnerText;
            var debutTradingDate = allTables.Nodes[2].SelectSingleNode("./tr[1]/td[2]").InnerText;
            var instrumentType = allTables.Nodes[2].SelectSingleNode("./tr[2]/td[2]").InnerText;
            var faceParValue = allTables.Nodes[2].SelectSingleNode("./tr[3]/td[1]").InnerText;
            var paidUpCapital = allTables.Nodes[2].SelectSingleNode("./tr[2]/td[1]").InnerText;
            var marketLot = allTables.Nodes[2].SelectSingleNode("./tr[3]/td[2]").InnerText;
            var totalOutstandingSecurity = allTables.Nodes[2].SelectSingleNode("./tr[4]/td[1]").InnerText;
            var sector = allTables.Nodes[2].SelectSingleNode("./tr[4]/td[2]").InnerText;

            var basicInfo = new BasicInfo
            {
                AuthorizedCapital = double.TryParse(authorizedCapital, out valF) ? valF : null,
                DebutTradingDate = debutTradingDate,
                InstrumentType = instrumentType,
                FaceParValue = double.TryParse(faceParValue, out valF) ? valF : null,
                PaidUpCapital = double.TryParse(paidUpCapital, out valF) ? valF : null,
                MarketLot = int.TryParse(marketLot, out valI) ? valI : null,
                TotalOutstandingSecurity = double.TryParse(totalOutstandingSecurity, out valF) ? valF : null,
                Sector = sector
            };

            return basicInfo;
        }
        public CompanyAddress GetCompanyAddress(TableWithHead allTables)
        {
            var addrHeadOffice = allTables.Nodes[12].SelectSingleNode("./tr[1]/td[3]").InnerText;
            var addrFactory = allTables.Nodes[12].SelectSingleNode("./tr[2]/td[2]").InnerText;
            var phone = allTables.Nodes[12].SelectSingleNode("./tr[3]/td[2]").InnerText;
            var fax = allTables.Nodes[12].SelectSingleNode("./tr[4]/td[2]").InnerText;
            var email = allTables.Nodes[12].SelectSingleNode("./tr[5]/td[2]").InnerText;
            var webAddress = allTables.Nodes[12].SelectSingleNode("./tr[6]/td[2]").InnerText.Trim();
            var secretaryName = allTables.Nodes[12].SelectSingleNode("./tr[7]/td[2]").InnerText;
            var secretaryMobile = allTables.Nodes[12].SelectSingleNode("./tr[8]/td[2]").InnerText;
            var secretaryPhone = allTables.Nodes[12].SelectSingleNode("./tr[9]/td[2]").InnerText;
            var secretaryEmail = allTables.Nodes[12].SelectSingleNode("./tr[10]/td[2]").InnerText;

            var companyAddress = new CompanyAddress
            {
                AddrHeadOffice = addrHeadOffice,
                AddrFactory = addrFactory,
                Phone = phone,
                Fax = fax,
                Email = email,
                WebAddress = webAddress,
                SecretaryName = secretaryName,
                SecretaryMobile = secretaryMobile,
                SecretaryPhone = secretaryPhone,
                SecretaryEmail = secretaryEmail
            };

            return companyAddress;
        }
        public OtherInfo GetOtherInfo(TableWithHead allTables)
        {
            int valI;

            var otherInfo = new OtherInfo();

            var listingYear = allTables.Nodes[10].SelectSingleNode("./tr[1]/td[2]").InnerText;

            otherInfo.ListingYear = int.TryParse(listingYear, out valI) ? valI : null;
            otherInfo.MarketCategory = allTables.Nodes[10].SelectSingleNode("./tr[2]/td[2]").InnerText;
            otherInfo.ElectronicShare = allTables.Nodes[10].SelectSingleNode("./tr[3]/td[2]").InnerText;

            var countRows = allTables.Nodes[10].SelectNodes("./tr").Count();

            string remarkPath = string.Format("./tr[{0}]/td[2]", countRows);

            otherInfo.Remarks = allTables.Nodes[10].SelectSingleNode(remarkPath).InnerText;

            return otherInfo;
        }
        public List<compData> GetCompanyLinks()
        {
            var html = @"https://www.dsebd.org/company_listing.php";

            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()).ToList();
            alpha.Add("Additional");

            HtmlWeb web = new HtmlWeb();

            var targetPage = web.Load(html);

            List<compData> compList = new List<compData>();

            foreach (var letter in alpha)
            {
                var xpath = String.Format("//*[@id='{0}']", letter);

                var targetLetterNodes = targetPage.DocumentNode.SelectNodes(xpath).Descendants().Where(n => n.HasClass("ab1"));

                if (targetLetterNodes != null)
                {
                    foreach (var targetLetterNode in targetLetterNodes)
                    {
                        var comp = new compData
                        {
                            CompName = targetLetterNode.InnerText,
                            Link = targetLetterNode.Attributes["href"].Value
                        };

                        compList.Add(comp);
                    }
                }
            }

            _logger.LogInformation("Found {0} Links to Scrap.", compList.Count());
            return compList;
        }
        public List<ShareHoldingPerct> GetShareHoldingPerct(TableWithHead allTables)
        {
            var count = allTables.Nodes[10].SelectNodes("./tr").Count();

            List<ShareHoldingPerct> listShare = new List<ShareHoldingPerct>();

            if (count > 4)
            {
                for (int i = 4; i < count; i++)
                {
                    double valF;
                    int valI;

                    var rowXPath = string.Format("./tr[{0}]", i);

                    //row1
                    var str1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[1]").InnerText;

                    if (str1 != null)
                    {
                        var year1 = Regex.Match(str1, @"\d{4}").Value;
                        var day1 = Regex.Match(str1, @"\d{2}").Value;
                        var month1 = Regex.Match(str1, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                        var sponsorDirector1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim();
                        var govt1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim();
                        var institute1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim();
                        var foreign1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim();
                        var public1 = allTables.Nodes[10].SelectSingleNode(rowXPath + "/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim();

                        var modelShare1 = new ShareHoldingPerct
                        {
                            Year = int.Parse(year1),
                            Day = int.Parse(day1),
                            Month = month1,
                            SponsorDirector = double.Parse(sponsorDirector1),
                            Govt = double.Parse(govt1),
                            Institute = double.Parse(institute1),
                            Foreign = double.Parse(foreign1),
                            Public = double.Parse(public1)
                        };

                        modelShare1.Date = new DateTime(modelShare1.Year, DateTime.ParseExact(modelShare1.Month, "MMM", CultureInfo.CurrentCulture).Month, modelShare1.Day);

                        listShare.Add(modelShare1);
                    }
                }
            }

            return listShare;
        }
        public MarketInfo GetMarketInfo(TableWithHead allTables)
        {
            double val;
            int val1;

            var DaysRange = allTables.Nodes[1].SelectSingleNode("./tr[2]/td[2]").InnerText.Split("-");
            var Weeks52range = allTables.Nodes[1].SelectSingleNode("./tr[4]/td[2]").InnerText.Split("-");

            var lastTradingPrice = allTables.Nodes[1].SelectSingleNode("./tr[1]/td[1]").InnerText;
            var closingPrice = allTables.Nodes[1].SelectSingleNode("./tr[1]/td[2]").InnerText;
            var openingPrice = allTables.Nodes[1].SelectSingleNode("./tr[5]/td[1]").InnerText;
            var daysVolume = allTables.Nodes[1].SelectSingleNode("./tr[5]/td[2]").InnerText;
            var daysValue = allTables.Nodes[1].SelectSingleNode("./tr[3]/td[2]").InnerText;
            var daysRangeMin = DaysRange[0].Trim();
            var daysRangeMax = DaysRange[1].Trim();
            var weaks52MovingRangeMin = Weeks52range[0].Trim();
            var weaks52MovingRangeMax = Weeks52range[1].Trim();
            var change = allTables.Nodes[1].SelectSingleNode("./tr[3]/td[1]").InnerText;
            var changePerct = allTables.Nodes[1].SelectSingleNode("./tr[4]/td[1]").InnerText.Replace("%", "");
            var openingPriceAdjusted = allTables.Nodes[1].SelectSingleNode("./tr[6]/td[1]").InnerText;
            var daysTrade = allTables.Nodes[1].SelectSingleNode("./tr[6]/td[2]").InnerText.Replace(",", "");
            var closingPriceYesterday = allTables.Nodes[1].SelectSingleNode("./tr[7]/td[1]").InnerText;
            var marketCapitalization = allTables.Nodes[1].SelectSingleNode("./tr[7]/td[2]").InnerText;

            var markInfo = new MarketInfo
            {
                LastTradingPrice = double.TryParse(lastTradingPrice, out val) ? val : null,
                ClosingPrice = double.TryParse(closingPrice, out val) ? val : null,
                OpeningPrice = double.TryParse(openingPrice, out val) ? val : null,
                DaysVolume = double.TryParse(daysVolume, out val) ? val : null,
                DaysValue = double.TryParse(daysValue, out val) ? val : null,
                DaysRangeMin = double.TryParse(daysRangeMin, out val) ? val : null,
                DaysRangeMax = double.TryParse(daysRangeMax, out val) ? val : null,
                Weaks52MovingRangeMin = double.TryParse(weaks52MovingRangeMin, out val) ? val : null,
                Weaks52MovingRangeMax = double.TryParse(weaks52MovingRangeMax, out val) ? val : null,
                Change = double.TryParse(change, out val) ? val : null,
                ChangePerct = double.TryParse(changePerct, out val) ? val : null,
                OpeningPriceAdjusted = double.TryParse(openingPriceAdjusted, out val) ? val : null,
                DaysTrade = int.TryParse(daysTrade, out val1) ? val1 : null,
                ClosingPriceYesterday = double.TryParse(closingPriceYesterday, out val) ? val : null,
                MarketCapitalization = double.TryParse(marketCapitalization, out val) ? val : null
            };

            return markInfo;
        }
        public CompanyTypeDto GetCompanyDetails(TableWithHead allTables)
        {
            var companyTypeDto = new CompanyTypeDto();

            string[] name = allTables.Head.Split(":");
            string[] code = allTables.Nodes[0].SelectSingleNode("//th[1]").InnerText.Split(":");
            string[] scrip = allTables.Nodes[0].SelectSingleNode("//th[2]").InnerText.Split(":");

            var type = name[0].Trim().Split(" ")[0].ToUpper();

            if (type == "SECURITY")
            {
                string[] isin = allTables.Nodes[0].SelectSingleNode("//th[3]").InnerText.Split(":");

                companyTypeDto.SecurityName = name[1].Trim().ToUpper();
                companyTypeDto.SecurityCode = code[1].Trim();
                companyTypeDto.ScripCode = scrip[1].Trim();
                companyTypeDto.Isin = isin[1].Trim().ToUpper();
                companyTypeDto.Type = type;
            }
            else
            {
                companyTypeDto.CompanyName = name[1].Trim().ToUpper();
                companyTypeDto.CompanyCode = code[1].Trim();
                companyTypeDto.ScripCode = scrip[1].Trim();
                companyTypeDto.Type = type;
            }

            return companyTypeDto;
        }


    }
}
