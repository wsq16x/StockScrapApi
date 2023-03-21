using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class MarketInfo
    {
        public int Id { get; set; }
        public double? LastTradingPrice { get; set; }
        //public DateOnly Date { get; set; }
        public double? ClosingPrice { get; set; }
        public double? OpeningPrice { get; set; }
        public double? OpeningPriceAdjusted { get; set; }
        public double? ClosingPriceYesterday { get; set; }
        public double? DaysValue { get; set; }
        public double? DaysRangeMin { get; set; }
        public double? DaysRangeMax { get; set; }
        public double? Weaks52MovingRangeMin { get; set; }
        public double? Weaks52MovingRangeMax { get; set; }
        public double? Change { get; set; }
        public double? ChangePerct { get; set; }
        public double? DaysVolume { get; set; }
        public int? DaysTrade { get; set; }
        public double? MarketCapitalization { get; set; }
        public DateTime TimeStamp { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
