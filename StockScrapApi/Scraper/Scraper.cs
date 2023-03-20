using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Models;
using System.Text.Json;
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
                    foreach(var item in shareHoldPerct)
                    {
                        var checkinfo = _context.shareHoldingPercts.Where(a=>a.Year == item.Year && a.Id == companyId && a.Month == item.Month).Any();
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

                var markInfo = new MarketInfo();

                var DaysRange = allTables.Nodes[1].SelectSingleNode("./tr[2]/td[2]").InnerText.Split("-");
                var Weeks52range = allTables.Nodes[1].SelectSingleNode("./tr[4]/td[2]").InnerText.Split("-");

                markInfo.LastTradingPrice = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[1]/td[1]").InnerText);
                markInfo.ClosingPrice = float.TryParse(allTables.Nodes[1].SelectSingleNode("./tr[1]/td[2]").InnerText, out val) ? val : null;
                markInfo.OpeningPrice = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[5]/td[1]").InnerText);
                markInfo.DaysVolume = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[5]/td[2]").InnerText);
                markInfo.DaysValue = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[3]/td[2]").InnerText);
                markInfo.DaysRangeMin = float.Parse(DaysRange[0].Trim());
                markInfo.DaysRangeMax = float.Parse(DaysRange[1].Trim());
                markInfo.Weaks52MovingRangeMin = float.Parse(Weeks52range[0].Trim());
                markInfo.Weaks52MovingRangeMax = float.Parse(Weeks52range[1].Trim());
                markInfo.change = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[3]/td[1]").InnerText);
                markInfo.changePerct = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[4]/td[1]").InnerText.Replace("%", ""));
                markInfo.OpeningPriceAdjusted = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[6]/td[1]").InnerText);
                markInfo.DaysTrade = int.Parse(allTables.Nodes[1].SelectSingleNode("./tr[6]/td[2]").InnerText.Replace(",", ""));
                markInfo.ClosingPriceYesterday = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[7]/td[1]").InnerText);
                markInfo.MarketCapitalization = float.Parse(allTables.Nodes[1].SelectSingleNode("./tr[7]/td[2]").InnerText);

                return markInfo;
            }

            BasicInfo GetBasicInfo(TableWithHead allTables)
            {
                var basicInfo = new BasicInfo();

                basicInfo.AuthorizedCapital = float.Parse(allTables.Nodes[2].SelectSingleNode("./tr[1]/td[1]").InnerText);
                basicInfo.DebutTradingDate = allTables.Nodes[2].SelectSingleNode("./tr[1]/td[2]").InnerText;
                basicInfo.PaidUpCapital = float.Parse(allTables.Nodes[2].SelectSingleNode("./tr[2]/td[1]").InnerText);
                basicInfo.InstrumentType = allTables.Nodes[2].SelectSingleNode("./tr[2]/td[2]").InnerText;
                basicInfo.FaceParValue = float.Parse(allTables.Nodes[2].SelectSingleNode("./tr[3]/td[1]").InnerText);
                basicInfo.PaidUpCapital = float.Parse(allTables.Nodes[2].SelectSingleNode("./tr[2]/td[1]").InnerText);
                basicInfo.MarketLot = int.Parse(allTables.Nodes[2].SelectSingleNode("./tr[3]/td[2]").InnerText);
                basicInfo.TotalOutstandingSecurity = float.Parse(allTables.Nodes[2].SelectSingleNode("./tr[4]/td[1]").InnerText);
                basicInfo.Sector = allTables.Nodes[2].SelectSingleNode("./tr[4]/td[2]").InnerText;

                return basicInfo;
            }

            CompanyAddress GetAddress(TableWithHead allTables)
            {
                var companyAddress = new CompanyAddress();

                companyAddress.AddrHeadOffice = allTables.Nodes[12].SelectSingleNode("./tr[1]/td[3]").InnerText;
                companyAddress.AddrFactory = allTables.Nodes[12].SelectSingleNode("./tr[2]/td[2]").InnerText;
                companyAddress.Phone = allTables.Nodes[12].SelectSingleNode("./tr[3]/td[2]").InnerText;
                companyAddress.Fax = allTables.Nodes[12].SelectSingleNode("./tr[4]/td[2]").InnerText;
                companyAddress.Email = allTables.Nodes[12].SelectSingleNode("./tr[5]/td[2]").InnerText;
                companyAddress.WebAddress = allTables.Nodes[12].SelectSingleNode("./tr[6]/td[2]").InnerText.Trim();
                companyAddress.SecretaryName = allTables.Nodes[12].SelectSingleNode("./tr[7]/td[2]").InnerText;
                companyAddress.SecretaryMobile = allTables.Nodes[12].SelectSingleNode("./tr[8]/td[2]").InnerText;
                companyAddress.SecretaryPhone = allTables.Nodes[12].SelectSingleNode("./tr[9]/td[2]").InnerText;
                companyAddress.SecretaryEmail = allTables.Nodes[12].SelectSingleNode("./tr[10]/td[2]").InnerText;

                return companyAddress;
            }

            OtherInfo GetOtherInfo(TableWithHead allTables)
            {
                var otherInfo = new OtherInfo();

                otherInfo.ListingYear = int.Parse(allTables.Nodes[10].SelectSingleNode("./tr[1]/td[2]").InnerText);
                otherInfo.MarketCategory = allTables.Nodes[10].SelectSingleNode("./tr[2]/td[2]").InnerText;
                otherInfo.ElectronicShare = allTables.Nodes[10].SelectSingleNode("./tr[3]/td[2]").InnerText;
                otherInfo.Remarks = allTables.Nodes[10].SelectSingleNode("./tr[7]/td[2]").InnerText;

                return otherInfo;
            }

            List<ShareHoldingPerct> GetShareHoldingPerct(TableWithHead allTables)
            {
                List<ShareHoldingPerct> listShare = new List<ShareHoldingPerct>();

                var modelShare1 = new ShareHoldingPerct();
                var modelShare2 = new ShareHoldingPerct();
                var modelShare3 = new ShareHoldingPerct();

                //row1
                var str1 = allTables.Nodes[10].SelectSingleNode("./tr[4]/td[1]").InnerText;
                modelShare1.Year = int.Parse(Regex.Match(str1, @"\d{4}").Value);
                modelShare1.Day = int.Parse(Regex.Match(str1, @"\d{2}").Value);
                modelShare1.Month = Regex.Match(str1, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                modelShare1.SponsorDirector = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim());
                modelShare1.Govt = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim());
                modelShare1.Institute = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim());
                modelShare1.Foreign = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim());
                modelShare1.Public = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[4]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim());
                listShare.Add(modelShare1);

                //row2
                var str2 = allTables.Nodes[10].SelectSingleNode("./tr[5]/td[1]").InnerText;
                modelShare2.Year = int.Parse(Regex.Match(str2, @"\d{4}").Value);
                modelShare2.Day = int.Parse(Regex.Match(str2, @"\d{2}").Value);
                modelShare2.Month = Regex.Match(str2, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                modelShare2.SponsorDirector = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim());
                modelShare2.Govt = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim());
                modelShare2.Institute = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim());
                modelShare2.Foreign = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim());
                modelShare2.Public = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[5]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim());
                listShare.Add(modelShare2);

                //row3
                var str3 = allTables.Nodes[10].SelectSingleNode("./tr[6]/td[1]").InnerText;
                modelShare3.Year = int.Parse(Regex.Match(str3, @"\d{4}").Value);
                modelShare3.Day = int.Parse(Regex.Match(str3, @"\d{2}").Value);
                modelShare3.Month = Regex.Match(str3, @"\b(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\b").Value;
                modelShare3.SponsorDirector = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[1]").InnerText.Split(":")[1].Trim());
                modelShare3.Govt = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[2]").InnerText.Split(":")[1].Trim());
                modelShare3.Institute = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[3]").InnerText.Split(":")[1].Trim());
                modelShare3.Foreign = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[4]").InnerText.Split(":")[1].Trim());
                modelShare3.Public = float.Parse(allTables.Nodes[10].SelectSingleNode("./tr[6]/td[2]/table/tr/td[5]").InnerText.Split(":")[1].Trim());
                listShare.Add(modelShare3);

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