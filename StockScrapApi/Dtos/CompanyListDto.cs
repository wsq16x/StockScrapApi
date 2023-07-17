using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class CompanyListDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ScripCode { get; set; }
        public string Url { get; set; }
        public string? LogoUrl { get; set; }
        public string? Type { get; set; }
    }
}