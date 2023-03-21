using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class Security
    {
        public int Id { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string ScripCode { get; set; }
        public string Isin { get; set; }
        public string Url { get; set; }
    }
}
