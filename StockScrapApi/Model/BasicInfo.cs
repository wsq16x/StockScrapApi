﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class BasicInfo
    {
        public int Id { get; set; }
        public float? AuthorizedCapital { get; set; }
        public string DebutTradingDate { get; set; }
        public float? PaidUpCapital { get; set; }
        public float? FaceParValue { get; set; }
        public float? TotalOutstandingSecurity { get; set; }
        public string InstrumentType { get; set; }
        public int MarketLot { get; set; }
        public string Sector { get; set; }
        public DateTime TimeStamp { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
