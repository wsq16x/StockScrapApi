using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

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
            var watch = new Stopwatch();
            var baseUrl = _configuration.GetValue<string>("BaseUrl");
            var directory = "/Images/Persons/Pictures/";

            watch.Start();

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
                            ImageUrl = a.profilePicture != null ? baseUrl + directory + a.profilePicture.ImagePath : null
                        }).ToListAsync();
            watch.Stop();
            _logger.LogInformation("Took {0}", watch.ElapsedMilliseconds);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetPersonsPaged")]
        public async Task<IActionResult> GetPersons(int? page, int? count)
        {
            return Ok();
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

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromForm]CreatePersonDto createPersonDto)
        {
            var person = _mapper.Map<Person>(createPersonDto);
            var ContentTypes = new List<string> { "image/png", "image/jpeg"};
            string? imagePath = null;
            string? fullImagePath = null;
            if (createPersonDto.Picture != null)
            { 
                var fileName = createPersonDto.Picture.FileName;
                if (!ContentTypes.Contains(createPersonDto.Picture.ContentType))
                {
                    return BadRequest("Only .png, .jpg and .jpeg extension allowed.");
                }

                var directory = _hostEnvironment.ContentRootPath + "Files/Images/Persons/Pictures";
                var GeneratedfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                var filePath = Path.Combine(directory, GeneratedfileName);
                
                using (var stream = System.IO.File.Create(filePath))
                {
                    await createPersonDto.Picture.CopyToAsync(stream);
                }

                imagePath = GeneratedfileName;

                _logger.LogInformation("Image saved at '{0}'", filePath);

            }

            try
            {
                _context.Add(person);
                await _context.SaveChangesAsync();

                if(createPersonDto.Picture != null)
                {
                    var profilePic = new ProfilePicture
                    {
                        PersonId = person.Id,
                        ImagePath = imagePath
                    };

                    _context.Add(profilePic);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error occured adding person.", ex.Message);
                if (fullImagePath != null)
                {
                    System.IO.File.Delete(fullImagePath);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed. Please try again.");
            }



            return Ok(person.Id);

        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpPut]
        public async Task<IActionResult> EditPerson(Guid Id, [FromForm]EditPersonDto editPersonDto)
        {

            if (!_context.persons.Where(x => x.Id == Id).Any() && Id == editPersonDto.Id)
            {
                return NotFound();
            }

            var person = _mapper.Map<Person>(editPersonDto);
            var ContentTypes = new List<string> { "image/png", "image/jpeg" };
            string? imagePath = null;
            string? fullImagePath = null;

            if (editPersonDto.Picture != null)
            {
                if (!ContentTypes.Contains(editPersonDto.Picture.ContentType))
                {
                    return BadRequest("Only .png, .jpg and .jpeg extension allowed.");
                }

                var directory = _hostEnvironment.ContentRootPath + "Files/Images/Persons/Pictures";

                var existingImage = await _context.profilePictures.Where(x => x.PersonId == editPersonDto.Id).Select(x => x.ImagePath).FirstOrDefaultAsync();
                if (existingImage != null)
                {
                    System.IO.File.Delete(Path.Combine(directory, existingImage));
                }

                var fileName = editPersonDto.Picture.FileName;

                var GeneratedfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                var filePath = Path.Combine(directory, GeneratedfileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await editPersonDto.Picture.CopyToAsync(stream);
                }

                imagePath = GeneratedfileName;

                _logger.LogInformation("Image saved at '{0}'", filePath);
            }


            try
            {
                _context.Update(person);

                if (editPersonDto.Picture != null)
                {
                    var profilePicture = await _context.profilePictures.Where(x => x.PersonId == person.Id).FirstOrDefaultAsync();
                    if (profilePicture != null)
                    {
                       profilePicture.ImagePath = imagePath;

                        _context.Update(profilePicture);
                    }
                    else
                    {
                        var profilePic = new ProfilePicture
                        {
                            PersonId = person.Id,
                            ImagePath = imagePath
                        };

                        _context.Add(profilePic);
                    }

                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error occured adding person.", ex.Message);
                if (fullImagePath != null)
                {
                    System.IO.File.Delete(fullImagePath);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed. Please try again.");
            }

            return CreatedAtRoute("GetPerson", new {id = person.Id}, person);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePerson(Guid Id)
        {
            return Ok();
        }
    }
}