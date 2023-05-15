using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Dtos;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return Ok();
        }
    }
}
