using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class CompanyTypeDto
    {
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ScripCode { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string Isin { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
    }
}
