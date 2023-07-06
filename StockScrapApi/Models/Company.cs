using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ScripCode { get; set; }
        public string Url { get; set; }
        public string? Type { get; set; }
        public CompanyAddress? CompanyAddress { get; set; }
        public OtherInfo? OtherInfo { get; set; }
        public CompanyLogo? CompanyLogo { get; set; }
        public BasicInfo? BasicInfo { get; set; }
        public List<Person>? Persons { get; set; }
        

    }
}
