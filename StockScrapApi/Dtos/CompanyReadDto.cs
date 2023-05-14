using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Dtos
{
    public class CompanyReadDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ScripCode { get; set; }
        public string Url { get; set; }      
        public CompanyAddress CompanyAddress { get; set; }
        public OtherInfo OtherInfo { get; set; }
        public BasicInfo BasicInfo { get; set; }
        public List<PersonInclude>? Persons { get; set; }
        public string? LogoUrl { get; set; }


    }
    public class PersonInclude
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string? ImageUrl { get; set; }
    }
}
