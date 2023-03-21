using AutoMapper;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using StockScrapApi.Types;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StockScrapApi.Scraper
{
    public class Scraper : IScraper
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<Scraper> _logger;
        private readonly IScrapeData _scrapeData;

        public Scraper(ApplicationDbContext context, IMapper mapper, ILogger<Scraper> logger, IScrapeData scrapeData)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _scrapeData = scrapeData;
        }

        public async Task ScrapeAndPush()
        {
            var rootUrl = @"https://www.dsebd.org";

            var compList = _scrapeData.GetCompanyLinks();

            GetAllCompInfo();

            Console.WriteLine(compList.Count);

            //parsing table
            async Task GetAllCompInfo()
            {
                foreach (var comp in compList)
                {
                    var CompCode = comp.CompName;
                    var allTables = _scrapeData.GetTables(rootUrl, comp.Link);

                    var checkComp = _context.companies.Where(t => t.CompanyCode == CompCode).Any();
                    var checkSec = _context.securities.Where(t => t.SecurityCode == CompCode).Any();
                    var companyTypeDto = _scrapeData.GetCompanyDetails(allTables);

                        if(companyTypeDto.Type == "SECURITY")
                        {
                            if (!checkSec)
                            {
                                var security = _mapper.Map<Security>(companyTypeDto);
                                security.Url = comp.Link;
                                _context.Add(security);
                                _context.SaveChanges();
                        }
                        }
                        else
                        {
                            if (!checkComp)
                            {
                                var company = _mapper.Map<Company>(companyTypeDto);
                                company.Url = comp.Link;
                                _context.Add(company);
                                _context.SaveChanges();
                            }
                        }     
                        
                    if(companyTypeDto.Type == "COMPANY")
                    {

                        var companyId = _context.companies.Where(t => t.CompanyCode == CompCode).Select(b => b.Id).FirstOrDefault();


                        if (!checkComp)
                        {
                            var checkAddr = _context.companyAddresses.Where(t => t.CompanyId == companyId).Any();

                            if(!checkAddr)
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

                            _context.SaveChanges();
                        }

                        var shareHoldPerct = _scrapeData.GetShareHoldingPerct(allTables);
                        foreach (var item in shareHoldPerct)
                        {
                            var checkinfo = _context.shareHoldingPercts.Where(a => a.Year == item.Year && a.Id == companyId && a.Month == item.Month).Any();
                            if (!checkinfo)
                            {
                                item.CompanyId = companyId;
                                _context.Add(item);
                            }
                        }

                        var marketInfo = _scrapeData.GetMarketInfo(allTables);
                        marketInfo.CompanyId = companyId;
                        _context.Add(marketInfo);

                        var BasicInfo = _scrapeData.GetBasicInfo(allTables);
                        BasicInfo.CompanyId = companyId;
                        _context.Add(BasicInfo);

                        _context.SaveChanges();

                    }//

                    _logger.LogInformation("Fetched Data for {0}", CompCode);
                }
            }
         
        }

    }
}