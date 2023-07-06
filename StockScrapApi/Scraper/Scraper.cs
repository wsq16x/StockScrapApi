using AutoMapper;
using Hangfire;
using HtmlAgilityPack;
using StockScrapApi.Data;
using StockScrapApi.Models;
using StockScrapApi.Types;

namespace StockScrapApi.Scraper
{
    public class Scraper : IScraper
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<Scraper> _logger;
        private readonly IScrapeData _scrapeData;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public Scraper(ApplicationDbContext context, IMapper mapper, ILogger<Scraper> logger, IScrapeData scrapeData, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _scrapeData = scrapeData;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task ScrapeAndPush(bool? automaticRetry = false, bool? forceScrape = false)
        {
            var defaultUrl = @"https://www.dsebd.org";
            var atbUrl = @"https://atb.dsebd.org";
            var smeUrl = @"https://sme.dsebd.org";

            var client = new HttpClient();

            var response = await client.GetAsync("https://www.dsebd.org/company_listing.php");

            //if (!response.IsSuccessStatusCode)
            //{
            //    if(automaticRetry == true)
            //    {
            //        var threshHold = TimeSpan.Parse("22:00:00");
            //        if (DateTime.Now.TimeOfDay < threshHold)
            //        {
            //            var JobId = _backgroundJobClient.Schedule(() => ScrapeAndPush(true, false), TimeSpan.FromMinutes(20));
            //            _logger.LogWarning("Website Unreachable, Will try again at {0}", DateTime.Now + TimeSpan.FromMinutes(20));
            //        }
            //    }

            //    return;
            //}

            var defaultList = _scrapeData.GetCompanyLinks();
            var smeList = _scrapeData.GetSmeLinks();
            var atbList = _scrapeData.GetAtbLinks();
            var timeStamp = DateTime.Now;

            //await GetAllCompInfo(defaultUrl, defaultList);
            await GetAllCompInfo(smeUrl, smeList, "sme");
            await GetAllCompInfo(atbUrl, atbList, "atb");

            //parsing table
            async Task GetAllCompInfo(string rootUrl, List<compData> compList, string? type = null)
            {
                foreach (var comp in compList)
                {
                    var CompCode = comp.CompName;


                    var allTables = _scrapeData.GetTables(rootUrl, comp.Link);



                    var checkComp = _context.companies.Where(t => t.CompanyCode == CompCode).Any();
                    var checkSec = _context.securities.Where(t => t.SecurityCode == CompCode).Any();
                    var companyTypeDto = _scrapeData.GetCompanyDetails(allTables);

                    if (companyTypeDto.Type == "SECURITY")
                    {
                        if (!checkSec)
                        {
                            var security = _mapper.Map<Security>(companyTypeDto);
                            security.Url = comp.Link;
                            _context.Add(security);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (!checkComp)
                        {
                            var company = _mapper.Map<Company>(companyTypeDto);
                            company.Url = comp.Link;
                            company.Type = type;
                            _context.Add(company);
                            await _context.SaveChangesAsync();
                        }
                    }

                    if (companyTypeDto.Type == "COMPANY")
                    {
                        var companyId = _context.companies.Where(t => t.CompanyCode == CompCode).Select(b => b.Id).FirstOrDefault();

                        var checkAddr = _context.companyAddresses.Where(t => t.CompanyId == companyId).Any();

                        if (!checkAddr)
                        {
                            var address = _scrapeData.GetCompanyAddress(allTables);
                            address.CompanyId = companyId;
                            _context.Add(address);
                        }

                        var checkOtherInfo = _context.otherInfo.Where(t => t.CompanyId == companyId).Any();

                        if (!checkOtherInfo)
                        {
                            var otherInfo = _scrapeData.GetOtherInfo(allTables);
                            otherInfo.CompanyId = companyId;
                            _context.Add(otherInfo);
                        }

                        var checkBasicInfo = _context.basicInfo.Where(t => t.CompanyId == companyId).Any();

                        if (!checkBasicInfo)
                        {
                            var BasicInfo = _scrapeData.GetBasicInfo(allTables);
                            BasicInfo.CompanyId = companyId;
                            BasicInfo.TimeStamp = DateTime.Now;

                            _context.Add(BasicInfo);
                        }

                        //_context.SaveChanges();

                        var shareHoldPerct = _scrapeData.GetShareHoldingPerct(allTables);
                        foreach (var item in shareHoldPerct)
                        {
                            var checkinfo = _context.shareHoldingPercts.Where(a => a.Year == item.Year && a.CompanyId == companyId && a.Month == item.Month).Any();
                            if (!checkinfo)
                            {
                                item.CompanyId = companyId;
                                _context.Add(item);
                            }
                        }

                        var checkMarketInfo = _context.marketInfo.Where(a => a.TimeStamp.Date == DateTime.Now.Date && a.CompanyId == companyId).Any();


                        if (forceScrape == true)
                        {
                            checkMarketInfo = false;
                        }

                        if (!checkMarketInfo)
                        {
                            var marketInfo = _scrapeData.GetMarketInfo(allTables);
                            marketInfo.CompanyId = companyId;
                            marketInfo.TimeStamp = DateTime.Now;
                            _context.Add(marketInfo);
                        }
                    }//

                    if (_context.ChangeTracker.HasChanges())
                    {
                        _logger.LogInformation("Fetched Data for {0}", CompCode);
                    }
                    else
                    {
                        _logger.LogInformation("Data Up-to-Date for {0}", CompCode);
                    }

                    await _context.SaveChangesAsync();
                }
            }

        }

    }
}