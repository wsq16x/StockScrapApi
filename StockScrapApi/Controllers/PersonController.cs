using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System.IO;

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
        private readonly ILogger<PersonController> _logger;

        public PersonController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IMapper mapper, IConfiguration configuration, ILogger<PersonController> logger)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var baseUrl = _configuration.GetValue<string>("BaseUrl");
            var directory = "/Images/Persons/Pictures/";
            var result = await _context.persons.Include("Company").Include("profilePicture")
                        .Select(a => new PersonDto {
                            Id = a.Id,
                            Name = a.Name,
                            Bio = a.Bio,
                            Phone = a.Phone,
                            Email = a.Email,
                            Designation = a.Designation,
                            CompanyId = a.CompanyId,
                            CompanyCode = a.Company.CompanyCode,
                            CompanyName = a.Company.CompanyName,
                            ImageUrl = a.profilePicture.ImagePath != null ? baseUrl + directory + a.profilePicture.ImagePath : null
                        }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{Id:Guid}", Name = "GetPerson")]
        public async Task<IActionResult> GetPerson(Guid Id)
        {
            var baseUrl = _configuration.GetValue<string>("BaseUrl");
            var directory = "/Images/Persons/Pictures/";
            var result = await _context.persons.Include("Company").Include("profilePicture").Where(a => a.Id == Id)
            .Select(a => new PersonDto
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio,
                Phone = a.Phone,
                Email = a.Email,
                Designation = a.Designation,
                CompanyId = a.CompanyId,
                CompanyCode = a.Company.CompanyCode,
                CompanyName = a.Company.CompanyName,
                ImageUrl = a.profilePicture.ImagePath != null ? baseUrl + directory + a.profilePicture.ImagePath : null
            }).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
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

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody]CreatePersonDto createPersonDto)
        {
            var person = _mapper.Map<Person>(createPersonDto);

            if (createPersonDto.Picture != null)
            {
                var fileName = createPersonDto.Picture.FileName;
                if (!fileName.EndsWith(".png") || !fileName.EndsWith(".jpg") || !fileName.EndsWith(".jpeg"))
                {
                    return BadRequest("Only .png, .jpg and .jpeg extension allowed.");
                }

                var directory = _hostEnvironment.ContentRootPath + "Files/Images/Persons/Pictures";
                var generatedFileName = Guid.NewGuid().ToString() + createPersonDto.Picture.FileName;
                _logger.LogInformation(fileName);

            }


            _context.Add(person);
            //await _context.SaveChangesAsync();




            return Ok(createPersonDto.Picture);

        }

        [HttpPost]
        [Route("test")]
        public async Task<IActionResult> CreatePersonForm([FromForm] CreatePersonDto createPersonDto)
        {
            var person = _mapper.Map<Person>(createPersonDto);
            //_context.Add(person);
            //await _context.SaveChangesAsync();

            if(createPersonDto.Picture != null)
            {
                var fileName = createPersonDto.Picture.FileName;
                _logger.LogInformation(fileName);
                
            }
            
            

            return Ok(createPersonDto.Picture);
        }

        [HttpPut]
        public async Task<IActionResult> EditPerson(Guid Id, [FromBody]CreatePersonDto createPersonDto)
        {

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePerson()
        {
            return Ok();
        }
    }
}