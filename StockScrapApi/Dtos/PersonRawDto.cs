using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class PersonRawDTO
    {
        public string Designation { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ImageUrl { get; set; }
        public string tradingCode { get; set; }
        public string? FirebaseCompId { get; set; }
    }
}
