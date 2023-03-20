﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ScripCode { get; set; }
        public string Url { get; set; }
    }
}
