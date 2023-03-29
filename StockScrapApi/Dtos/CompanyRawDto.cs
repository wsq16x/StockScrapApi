using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class CompanyRawDto
    {
        public Guid CompanyId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
