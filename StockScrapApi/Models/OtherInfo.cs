using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class OtherInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int? ListingYear { get; set; }
        public string MarketCategory { get; set; }
        public string ElectronicShare { get; set; }
        public string? Remarks { get; set; }

        public Guid CompanyId { get; set; }
        //public Company Company { get; set; }
    }
}
