using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
