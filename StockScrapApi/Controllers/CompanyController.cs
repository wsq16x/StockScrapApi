using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyController> _logger;
        private readonly IMapper _mapper;

        public CompanyController(ApplicationDbContext context, ILogger<CompanyController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompany()
        {
            var results = await _context.companies.ToListAsync();

            return Ok(results);
        }

        [HttpGet]
        [Route("GetCompaniesWithInfo")]
        public async Task<IActionResult> GetAllCompanyWithInfo()
        {
            var results = await _mapper.ProjectTo<CompanyReadDto>(_context.companies).ToListAsync();

            return Ok(results);
        }

        [HttpGet]
        [Route("logo")]
        public async Task<IActionResult> GetCompanyLogoById(Guid Id)
        {
            var absPath = @"C:\Users\wasiq\source\repos\StockScrapApi\StockScrapApi\wwwroot\";
            var path = await _context.companyLogos.Where(a => a.CompanyId == Id).Select(b => b.LogoPath).FirstOrDefaultAsync();

            return File(Path.GetRelativePath(absPath, path), "image/jpg");

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
