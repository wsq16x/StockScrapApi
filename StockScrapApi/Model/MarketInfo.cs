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
        public float? LastTradingPrice { get; set; }
        //public DateOnly Date { get; set; }
        public float? ClosingPrice { get; set; }
        public float? OpeningPrice { get; set; }
        public float? OpeningPriceAdjusted { get; set; }
        public float? ClosingPriceYesterday { get; set; }
        public float? DaysValue { get; set; }
        public float? DaysRangeMin { get; set; }
        public float? DaysRangeMax { get; set; }
        public float? Weaks52MovingRangeMin { get; set; }
        public float? Weaks52MovingRangeMax { get; set; }
        public float? Change { get; set; }
        public float? ChangePerct { get; set; }
        public float? DaysVolume { get; set; }
        public int? DaysTrade { get; set; }
        public float? MarketCapitalization { get; set; }
        public DateTime TimeStamp { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
