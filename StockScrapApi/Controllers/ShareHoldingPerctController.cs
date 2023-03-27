using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShareHoldingPerctController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShareHoldingPerctController> _logger;
        private readonly IMapper _mapper;

        public ShareHoldingPerctController(ApplicationDbContext context, ILogger<ShareHoldingPerctController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllShareHoldingPerct()
        {
            var shareHoldingPerct = await _context.shareHoldingPercts.ToListAsync();
            
            var results = _mapper.Map<IList<ShareHoldingPerctReadDto>>(shareHoldingPerct);

            return Ok(results);
        }
    }
}
