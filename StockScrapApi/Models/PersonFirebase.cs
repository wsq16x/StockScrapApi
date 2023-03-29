using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class PersonFirebase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string PersonId { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? imageUrl { get; set; }
        public string? CompanyId { get; set; }
    }
}
