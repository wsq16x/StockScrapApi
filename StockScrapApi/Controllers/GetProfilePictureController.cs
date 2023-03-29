﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetProfilePictureById(Guid Id)
        {
            var absPath = @"C:\Users\wasiq\source\repos\StockScrapApi\StockScrapApi\wwwroot\";
            var path = await _context.profilePictures.Where(a => a.PersonId == Id).Select(b => b.ImagePath).FirstOrDefaultAsync();

            return File(Path.GetRelativePath(absPath, path), "image/jpg");
        }
    }
}
