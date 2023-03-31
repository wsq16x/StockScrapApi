using Hangfire;
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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public Initialize(ApplicationDbContext context, IGetFirebaseData getFirebaseData, IScraper scraper, IMapFirebaseData mapFirebaseData, IBackgroundJobClient backgroundJobClient)
        {
            _context = context;
            _getFirebaseData = getFirebaseData;
            _scraper = scraper;
            _mapFirebaseData = mapFirebaseData;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task InitDatabase()
        {
            var checkCompany = _context.companies.Any();
            var checkPerson = _context.persons.Any();

            if (!checkCompany)
            {
                await _scraper.ScrapeAndPush();
            }
            
            if (!checkPerson)
            {
                var fetch = await _getFirebaseData.FetchData();
                if (fetch)
                {
                    var result = await _mapFirebaseData.MoveData();

                    if (result)
                    {
                        var JobGetPorfilePictures = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetProfilePictures());
                        var JobGetCompanyLogo = _backgroundJobClient.Enqueue(() => _mapFirebaseData.GetCompanyLogo());
                    }
                }
                //await _mapFirebaseData.MoveData();
            }
            //await _mapFirebaseData.GetCompanyLogo();

        }
    }
}
