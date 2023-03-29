using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class CompanyFirebase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string companyID { get; set; }
        public string companyName { get; set; }
        public string scripCode { get; set; }
        public string? logo { get; set; }
        public string siteLink { get; set; }
        public DateTime? time { get; set; }
        public string tradingCode { get; set; }
    }
}
