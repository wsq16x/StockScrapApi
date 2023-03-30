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
        private readonly IWebHostEnvironment _hostEnvironment;

        public PersonController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
            var absPath = _hostEnvironment.WebRootPath;
            var path = await _context.profilePictures.Where(a => a.PersonId == Id).Select(b => b.ImagePath).FirstOrDefaultAsync();

            return File(Path.GetRelativePath(absPath, path), "image/jpg");
        }
    }
}