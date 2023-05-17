using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,SuperUser")]
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _context.Roles.Select(x => x.Name).ToListAsync();
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
