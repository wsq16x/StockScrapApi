using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Data;
using StockScrapApi.Models;
using System.Text.RegularExpressions;

namespace StockScrapApi.Scraper
{
    public class Scraper : IScraper
    {
        private readonly ApplicationDbContext _context;

        public Scraper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void ScrapeAndPush()
        {
            var rootUrl = @"https://www.dsebd.org";
            var html = @"https://www.dsebd.org/company_listing.php";

            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().Select(c => c.ToString()).ToList();
            alpha.Add("Additional");

            HtmlWeb web = new HtmlWeb();

            var targetPage = web.Load(html);

            //var targetBody = targetPage.DocumentNode.SelectNodes("//*[@id='RightBody']");

            var AllData = "";

            List<compData> compList = new List<compData>();

            foreach (var letter in alpha)
            {
                var xpath = String.Format("//*[@id='{0}']", letter);

                var targetLetterNodes = targetPage.DocumentNode.SelectNodes(xpath).Descendants().Where(n => n.HasClass("ab1"));

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

            GetAllCompInfo();

            Console.WriteLine(compList.Count);

            //parsing table
            async void GetAllCompInfo()
            {
                foreach (var comp in compList)
                {
                    var CompCode = comp.CompName;
                    var allTables = GetTables(comp.Link);

                    var check = _context.companies.Where(t => t.CompanyCode == CompCode).Any();

                    if (!check)
                    {
                        var company = GetCompany(allTables);
                        company.Url = comp.Link;

                        _context.Add(company);
                        _context.SaveChanges();
                    }

                    var companyId = _context.companies.Where(t => t.CompanyCode == CompCode).Select(b => b.Id).FirstOrDefault();

                    //
                    if (!check)
                    {
                        var address = GetAddress(allTables);

                        address.CompanyId = companyId;

                        var otherInfo = GetOtherInfo(allTables);

                        otherInfo.CompanyId = companyId;

                        _context.Add(address);
                        _context.Add(otherInfo);

                        _context.SaveChanges();
                    }

                    var shareHoldPerct = GetShareHoldingPerct(allTables);
                    foreach (var item in shareHoldPerct)
                    {
                        var checkinfo = _context.shareHoldingPercts.Where(a => a.Year == item.Year && a.Id == companyId && a.Month == item.Month).Any();
                        if (!checkinfo)
                        {
                            item.CompanyId = companyId;
                            _context.Add(item);
                        }
                    }

                    var marketInfo = GetMarketInformation(allTables);
                    marketInfo.CompanyId = companyId;
                    _context.Add(marketInfo);

                    var BasicInfo = GetBasicInfo(allTables);
                    BasicInfo.CompanyId = companyId;
                    _context.Add(BasicInfo);

                    _context.SaveChanges();
                }
            }

            TableWithHead GetTables(string url)
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

            //for the Company Table
            Company GetCompany(TableWithHead allTables)
            {
                var company = new Company();

                string[] name = allTables.Head.Split(":");
                string[] code = allTables.Nodes[0].SelectSingleNode("//th[1]").InnerText.Split(":");
                string[] scrip = allTables.Nodes[0].SelectSingleNode("//th[2]").InnerText.Split(":");

                company.CompanyName = name[1].Trim().ToUpper();
                company.CompanyCode = code[1].Trim();
                company.ScripCode = scrip[1].Trim();

                return company;
            }

            MarketInfo GetMarketInformation(TableWithHead allTables)
            {
                float val;
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
                    LastTradingPrice = float.TryParse(lastTradingPrice, out val) ? val : null,
                    ClosingPrice = float.TryParse(closingPrice, out val) ? val : null,
                    OpeningPrice = float.TryParse(openingPrice, out val) ? val : null,
                    DaysVolume = float.TryParse(daysVolume, out val) ? val : null,
                    DaysValue = float.TryParse(daysValue, out val) ? val : null,
                    DaysRangeMin = float.TryParse(daysRangeMin, out val) ? val : null,
                    DaysRangeMax = float.TryParse(daysRangeMax, out val) ? val : null,
                    Weaks52MovingRangeMin = float.TryParse(weaks52MovingRangeMin, out val) ? val : null,
                    Weaks52MovingRangeMax = float.TryParse(weaks52MovingRangeMax, out val) ? val : null,
                    Change = float.TryParse(change, out val) ? val : null,
                    ChangePerct = float.TryParse(changePerct, out val) ? val : null,
                    OpeningPriceAdjusted = float.TryParse(openingPriceAdjusted, out val) ? val : null,
                    DaysTrade = int.TryParse(daysTrade, out val1) ? val1 : null,
                    ClosingPriceYesterday = float.TryParse(closingPriceYesterday, out val) ? val : null,
                    MarketCapitalization = float.TryParse(marketCapitalization, out val) ? val : null
                };

                return markInfo;
            }

            BasicInfo GetBasicInfo(TableWithHead allTables)
            {
                float valF;
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
                    AuthorizedCapital = float.TryParse(authorizedCapital, out valF) ? valF : null,
                    DebutTradingDate = debutTradingDate,
                    InstrumentType = instrumentType,
                    FaceParValue = float.TryParse(faceParValue, out valF) ? valF : null,
                    PaidUpCapital = float.TryParse(paidUpCapital, out valF) ? valF : null,
                    MarketLot = int.TryParse(marketLot, out valI) ? valI : null,
                    TotalOutstandingSecurity = float.TryParse(totalOutstandingSecurity, out valF) ? valF : null,
                    Sector = sector
                };

                return basicInfo;
            }

            CompanyAddress GetAddress(TableWithHead allTables)
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

            OtherInfo GetOtherInfo(TableWithHead allTables)
            {
                var otherInfo = new OtherInfo();

                otherInfo.ListingYear = int.Parse(allTables.Nodes[10].SelectSingleNode("./tr[1]/td[2]").InnerText);
                otherInfo.MarketCategory = allTables.Nodes[10].SelectSingleNode("./tr[2]/td[2]").InnerText;
                otherInfo.ElectronicShare = allTables.Nodes[10].SelectSingleNode("./tr[3]/td[2]").InnerText;
                var RemarkNode = allTables.Nodes[10].SelectNodes("./tr").Where(a => a.InnerText.Contains("Remarks"));

                return otherInfo;
            }

            List<ShareHoldingPerct> GetShareHoldingPerct(TableWithHead allTables)
            {
                float valF;
                int valI;

                List<ShareHoldingPerct> listShare = new List<ShareHoldingPerct>();

                //row1
                var str1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[1]").InnerText;

                if(str1 != null)
                {
                    var year1 = Regex.Match(str1, @"\d{4}").Value;
                    var day1 = Regex.Match(str1, @"\d{2}").Value;
                    var month1 = Regex.Match(str1, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                    var sponsorDirector1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim();
                    var govt1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim();
                    var institute1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim();
                    var foreign1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim();
                    var public1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim();

                    var modelShare1 = new ShareHoldingPerct
                    {
                        Year = int.Parse(year1),
                        Day = int.Parse(day1),
                        Month = month1,
                        SponsorDirector = float.Parse(sponsorDirector1),
                        Govt = float.Parse(govt1),
                        Institute = float.Parse(institute1),
                        Foreign = float.Parse(foreign1),
                        Public = float.Parse(public1)
                    };

                    listShare.Add(modelShare1);
                }


                //row2
                var str2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[1]").InnerText;

                if(str2 != null)
                {
                    var year2 = Regex.Match(str2, @"\d{4}").Value;
                    var day2 = Regex.Match(str2, @"\d{2}").Value;
                    var month2 = Regex.Match(str2, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                    var sponsorDirector2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim();
                    var govt2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim();
                    var institute2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim();
                    var foreign2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim();
                    var public2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim();

                    var modelShare2 = new ShareHoldingPerct
                    {
                        Year = int.Parse(year2),
                        Day = int.Parse(day2),
                        Month = month2,
                        SponsorDirector = float.Parse(sponsorDirector2),
                        Govt = float.Parse(govt2),
                        Institute = float.Parse(institute2),
                        Foreign = float.Parse(foreign2),
                        Public = float.Parse(public2)
                    };

                    listShare.Add(modelShare2);
                }


                //row3
                var str3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[1]").InnerText;

                if(str3 != null)
                {
                    var year3 = Regex.Match(str3, @"\d{4}").Value;
                    var day3 = Regex.Match(str3, @"\d{2}").Value;
                    var month3 = Regex.Match(str3, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                    var sponsorDirector3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim();
                    var govt3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim();
                    var institute3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim();
                    var foreign3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim();
                    var public3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim();

                    var modelShare3 = new ShareHoldingPerct
                    {
                        Year = int.Parse(year3),
                        Day = int.Parse(day3),
                        Month = month3,
                        SponsorDirector = float.Parse(sponsorDirector3),
                        Govt = float.Parse(govt3),
                        Institute = float.Parse(institute3),
                        Foreign = float.Parse(foreign3),
                        Public = float.Parse(public3)
                    };

                    listShare.Add(modelShare3);
                }

                return listShare;
            }
        }

        private class compData
        {
            public string Link { get; set; }
            public string CompName { get; set; }
        }

        private class TableWithHead
        {
            public string Head { get; set; }
            public HtmlNodeCollection Nodes { get; set; }
        }
    }
}