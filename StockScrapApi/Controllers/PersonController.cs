using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PersonController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var result = await _context.persons.ToListAsync();

            return Ok(result);
        }

        [HttpGet("{Id:Guid}", Name = "GetPerson")]
        public async Task<IActionResult> GetPerson(Guid Id)
        {
            var person = await _context.persons.Include(a => a.Company).Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync();

            if (person == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<PersonDto>(person);

            var imgPath = await _context.profilePictures.Where(x => x.PersonId.Equals(Id)).Select(a => a.ImagePath).FirstOrDefaultAsync();

            if (imgPath != null)
            {
                result.PicturePath = String.Format(_configuration.GetValue<string>("BaseUrl"), "/Images/Persons/Pictures/", imgPath);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("ProfilePicture")]
        public async Task<IActionResult> GetProfilePictureById(Guid Id)
        {
            var fileName = await _context.profilePictures.Where(a => a.PersonId == Id).Select(b => b.ImagePath).FirstOrDefaultAsync();
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "Files", "Images", "Persons", "Pictures", fileName);
            if (fileName == null || !System.IO.File.Exists(path))
            {
                return NotFound();
            }
            return PhysicalFile(path, "image/jpg");
        }
    }
}