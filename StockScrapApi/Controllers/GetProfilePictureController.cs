using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockScrapApi.Data;

namespace StockScrapApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetProfilePictureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GetProfilePictureController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetProfilePictureById(string Id)
        {
            var path = string.Format("~/Images/Persons/{0}.jpg", Id);
            return File(path, "image/jpg");
        }
    }
}
