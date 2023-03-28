using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class ProfilePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? ImagePath { get; set; }
        public Guid PersonId { get; set; }
        public Person Person{ get; set; }
    }
}
