using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string Designation { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Company? Company { get; set; }
        public string? PicturePath { get; set; }
    }
}
