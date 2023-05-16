using System.ComponentModel.DataAnnotations;

namespace StockScrapApi.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        [Required]
        public ICollection<string> Roles { get; set; }
    }
}
