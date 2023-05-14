using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StockScrapApi.Data;
using System.Globalization;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MarketInfoController> _logger;
        private readonly IMapper _mapper;

        public MarketInfoController(ApplicationDbContext context, ILogger<MarketInfoController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMarketInfo()
        {
            var marketInfo = await _context.marketInfo.ToListAsync();
            return Ok(marketInfo);
        }

        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetMarketInfoById(Guid Id, [FromQuery]string? date)
        {
            if (date == null)
            {
                var resultAll = await _context.marketInfo.Where(x => x.CompanyId == Id).ToListAsync();

                if (resultAll == null)
                {
                    return NoContent();
                }
                return Ok(resultAll);
            }

            DateTime parsedDate;

            var parseResult = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate);

            if (!parseResult)
            {
                return BadRequest("The date must be in 'YYYY-MM-dd' format.");
            }

            var result = await _context.marketInfo.Where(x => x.CompanyId == Id && x.TimeStamp.Date == parsedDate.Date).FirstOrDefaultAsync();

            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        
    }
}
