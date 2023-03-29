using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyLogoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyLogoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyLogoById(Guid Id)
        {
            var absPath = @"C:\Users\wasiq\source\repos\StockScrapApi\StockScrapApi\wwwroot\";
            var path = await _context.companyLogos.Where(a => a.CompanyId == Id).Select(b => b.LogoPath).FirstOrDefaultAsync();

            return File(Path.GetRelativePath(absPath, path), "image/jpg");

        }
    }
}
