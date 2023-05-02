using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class ShareHoldingPerct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public double? SponsorDirector { get; set; }
        public double? Govt { get; set; }
        public double? Institute { get; set; }
        public double? Foreign { get; set; }
        public double? Public { get; set; }
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public int Day { get; set; }
        public DateTime TimeStamp { get; set; }

        public Guid CompanyId { get; set; }
        //public Company Company { get; set; }
    }
}
