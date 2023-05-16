using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockScrapApi.Models
{
    public class ApiUser : IdentityUser
    {
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
