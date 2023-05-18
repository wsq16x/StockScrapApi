using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;

        public CompanyController(ApplicationDbContext context, ILogger<CompanyController> logger, IMapper mapper, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var baseUrl = _configuration.GetValue<string>("BaseUrl");
            var directory = "/Images/Companies/Logos/";

            var dsebdUrl = _configuration.GetValue<string>("DsebdUrl");
            var results = await _context.companies.Include("CompanyLogo")
                .Select(x => new CompanyListDto
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    CompanyCode = x.CompanyCode,
                    ScripCode = x.ScripCode,
                    Url = dsebdUrl + x.Url,
                    LogoUrl = x.CompanyLogo != null ? baseUrl + directory + x.CompanyLogo.LogoPath : null
                }).ToListAsync();

            return Ok(results);
        }


        [HttpGet]
        [Route("GetCompaniesWithInfo")]
        public async Task<IActionResult> GetAllCompanyWithInfo()
        {
            var results = await _mapper.ProjectTo<CompanyReadDto>(_context.companies).ToListAsync();

            return Ok(results);
        }

        [HttpGet("{Id:Guid}", Name = "GetCompany")]
        public async Task<IActionResult> GetCompany(Guid Id)
        {
            var baseUrl = _configuration.GetValue<string>("BaseUrl");
            var directory = "/Images/Companies/Logos/";
            var directoryPerson = "/Images/Persons/Pictures/";

            var dsebdUrl = _configuration.GetValue<string>("DsebdUrl");

            var result = await _context.companies.Where(x => x.Id == Id).Include("CompanyAddress").Include("BasicInfo").Include("OtherInfo").Include("CompanyLogo").Include("Persons")
                .Select(a => new CompanyReadDto
                {
                    Id = a.Id,
                    CompanyName = a.CompanyName,
                    CompanyCode = a.CompanyCode,
                    ScripCode = a.ScripCode,
                    Url = dsebdUrl + a.Url,
                    BasicInfo = a.BasicInfo,
                    CompanyAddress = a.CompanyAddress,
                    OtherInfo = a.OtherInfo,
                    LogoUrl = a.CompanyLogo != null ? baseUrl + directory + a.CompanyLogo.LogoPath : null,
                    Persons = a.Persons.Select(p => new PersonInclude
                    {
                        Id=p.Id,
                        Name = p.Name,
                        Designation = p.Designation,
                        ImageUrl = p.profilePicture != null ? baseUrl + directoryPerson + p.profilePicture.ImagePath : null
                    }).ToList()

                }).FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("logo")]
        public async Task<IActionResult> GetCompanyLogoById(Guid Id)
        {
            var fileName = await _context.companyLogos.Where(a => a.CompanyId == Id).Select(b => b.LogoPath).FirstOrDefaultAsync();
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "Files", "Images", "Companies", "Logos", fileName);

            if (fileName == null || !System.IO.File.Exists(path))
            {
                return NotFound();
            }
            return PhysicalFile(path, "image/jpg");
        }

        [HttpPost]
        [Route("logo")]
        public async Task<IActionResult> AddCompanyLogo([FromForm] CompanyLogoDto companyLogoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var ContentTypes = new List<string> { "image/png", "image/jpeg" };
            string? imagePath = null;
            string? fullImagePath = null;

            if (companyLogoDto.Logo != null)
            {
                var fileName = companyLogoDto.Logo.FileName;
                if (!ContentTypes.Contains(companyLogoDto.Logo.ContentType))
                {
                    return BadRequest("Only .png, .jpg and .jpeg extension allowed.");
                }

                var directory = _hostEnvironment.ContentRootPath + "Files/Images/Companies/Logos";
                var GeneratedfileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

                var filePath = Path.Combine(directory, GeneratedfileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await companyLogoDto.Logo.CopyToAsync(stream);
                }

                imagePath = GeneratedfileName;

                _logger.LogInformation("Image saved at '{0}'", filePath);

            }


            var exisitingCompany = await _context.companyLogos.Where(x => x.CompanyId == companyLogoDto.Id).FirstOrDefaultAsync();
            

            if (exisitingCompany != null)
            {
                return BadRequest("Logo exists. Please delete first.");
            }
            
            
            var logo = new CompanyLogo
            {
                CompanyId = companyLogoDto.Id,
                LogoPath = imagePath
            };

            try
            {
                _context.Add(logo);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured adding logo.", ex.Message);
                if (fullImagePath != null)
                {
                    System.IO.File.Delete(fullImagePath);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed. Please try again.");
            }

            return NoContent();

        }

        [HttpDelete]
        [Route("logo")]
        public async Task<IActionResult> DeleteLogo(Guid Id)
        { 
            var companyLogo = await _context.companyLogos.Where(x => x.CompanyId == Id).FirstOrDefaultAsync();

            if (companyLogo == null)
            {
                return NotFound();
            }

            var directory = _hostEnvironment.ContentRootPath + "Files/Images/Companies/Logos";
            var fileName = companyLogo.LogoPath;

            var filePath = Path.Combine(directory, fileName);

            try
            {
                _context.Remove(companyLogo);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete logo. Please try again later.");
            }

            System.IO.File.Delete(filePath);

            return NoContent();
        }

        [HttpGet]
        [Route("Address")]
        public async Task<IActionResult> GetAllCompanyAdress()
        {
            var results = await _context.companyAddresses.ToListAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("BasicInfo")]
        public async Task<IActionResult> GetAllBasicInfo()
        {
            var results = await _context.basicInfo.ToListAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("OtherInfo")]
        public async Task<IActionResult> GetAllOtherInfo()
        {
            var otherInfo = await _context.otherInfo.ToListAsync();

            return Ok(otherInfo);
        }
    }
}