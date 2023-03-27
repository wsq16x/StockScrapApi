using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class ShareHoldingPerctReadDto
    {
        public Guid Id { get; set; }
        public double? SponsorDirector { get; set; }
        public double? Govt { get; set; }
        public double? Institute { get; set; }
        public double? Foreign { get; set; }
        public double? Public { get; set; }
        public DateTime Date { get; set; }
        //public string Month { get; set; }
        //public int Year { get; set; }
        //public int Day { get; set; }
        //public DateTime TimeStamp { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
