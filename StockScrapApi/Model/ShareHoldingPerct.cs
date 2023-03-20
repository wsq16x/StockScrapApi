using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class ShareHoldingPerct
    {
        public int Id { get; set; }
        public float? SponsorDirector { get; set; }
        public float? Govt { get; set; }
        public float? Institute { get; set; }
        public float? Foreign { get; set; }
        public float? Public { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int Day { get; set; }
        public DateTime TimeStamp { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
