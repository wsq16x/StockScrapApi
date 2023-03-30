using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PersonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var result = await _context.persons.ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("ProfilePicture")]
        public async Task<IActionResult> GetProfilePictureById(Guid Id)
        {
            var absPath = @"C:\Users\wasiq\source\repos\StockScrapApi\StockScrapApi\wwwroot\";
            var path = await _context.profilePictures.Where(a => a.PersonId == Id).Select(b => b.ImagePath).FirstOrDefaultAsync();

            return File(Path.GetRelativePath(absPath, path), "image/jpg");
        }
    }
}