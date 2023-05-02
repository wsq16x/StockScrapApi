using StockScrapApi.Models;

namespace StockScrapApi.Dtos
{
    public class CreatePersonDto
    {
        public string Designation { get; set; }
        public string Name { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public Guid CompanyId { get; set; }
        public IFormFile? Picture { get; set; }
    }

    public class EditPersonDto : CreatePersonDto
    { 
        public Guid Id { get; set; }
    }

}
