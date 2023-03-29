using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Data;
using StockScrapApi.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Helpers
{
    public class Initialize : IInitialize
    {
        private readonly ApplicationDbContext _context;
        private readonly IGetFirebaseData _getFirebaseData;
        private readonly IScraper _scraper;
        private readonly IMapFirebaseData _mapFirebaseData;

        public Initialize(ApplicationDbContext context, IGetFirebaseData getFirebaseData, IScraper scraper, IMapFirebaseData mapFirebaseData)
        {
            _context = context;
            _getFirebaseData = getFirebaseData;
            _scraper = scraper;
            _mapFirebaseData = mapFirebaseData;
        }

        public async Task InitDatabase()
        {
            var checkCompany = _context.companies.Any();
            var checkPerson = _context.personsFirebase.Any();

            if (!checkCompany)
            {
                await _scraper.ScrapeAndPush();
            }
            
            if (!checkPerson)
            {
                await _getFirebaseData.FetchData();
                await _mapFirebaseData.MoveData();
                await _mapFirebaseData.GetProfilePictures();
            }

            await _mapFirebaseData.GetCompanyLogo();

        }
    }
}
