using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Model
{
    public class ScrapeInfo
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public String CompanyCode { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
