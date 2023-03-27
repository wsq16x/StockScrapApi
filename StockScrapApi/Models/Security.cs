using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class Security
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string SecurityName { get; set; }
        public string SecurityCode { get; set; }
        public string ScripCode { get; set; }
        public string Isin { get; set; }
        public string Url { get; set; }
    }
}
