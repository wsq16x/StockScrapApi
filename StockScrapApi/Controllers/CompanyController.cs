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
            var results = await _mapper.ProjectTo<CompanyReadDto>(_context.companies).ToListAsync();

            return Ok(results);
        }
    }
}
