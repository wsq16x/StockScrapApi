using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Models
{
    public class CompanyAddress
    {
        public int Id { get; set; }
        public string AddrHeadOffice { get; set; }
        public string AddrFactory { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string  SecretaryName { get; set; }
        public string SecretaryPhone { get; set; }
        public string SecretaryMobile  { get; set;}
        public string SecretaryEmail { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
