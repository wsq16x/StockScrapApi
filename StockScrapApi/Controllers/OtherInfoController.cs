using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtherInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OtherInfoController> _logger;
        private readonly IMapper _mapper;

        public OtherInfoController(ApplicationDbContext context, ILogger<OtherInfoController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOtherInfo()
        {
            var otherInfo = await _context.otherInfo.ToListAsync();

            return Ok(otherInfo);
        }
    }
}
