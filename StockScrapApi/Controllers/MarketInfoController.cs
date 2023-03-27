using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;

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
    }
}
