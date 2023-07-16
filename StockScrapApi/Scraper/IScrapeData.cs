using HtmlAgilityPack;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using StockScrapApi.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Scraper
{
    public interface IScrapeData
    {
        TableWithHead GetTables(string rootUrl, string url, string? type);

        List<compData> GetCompanyLinks();

        List<compData> GetAtbLinks();

        List<compData> GetSmeLinks();

        BasicInfo GetBasicInfo(TableWithHead allTables);

        CompanyTypeDto GetCompanyDetails(TableWithHead allTables);

        MarketInfo GetMarketInfo(TableWithHead allTables);

        OtherInfo GetOtherInfo(TableWithHead allTables);

        List<ShareHoldingPerct> GetShareHoldingPerct(TableWithHead allTables);
       
        CompanyAddress GetCompanyAddress(TableWithHead allTables, string? type);
    }
}