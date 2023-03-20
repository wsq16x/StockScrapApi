using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class OtherInfo
    {
        public int Id { get; set; }
        public int ListingYear { get; set; }
        public string MarketCategory { get; set; }
        public string ElectronicShare { get; set; }
        public string? Remarks { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
