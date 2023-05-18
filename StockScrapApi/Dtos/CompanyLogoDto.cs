using System.ComponentModel.DataAnnotations;

namespace StockScrapApi.Dtos
{
    public class CompanyLogoDto
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public IFormFile Logo { get; set; }
    }
}
