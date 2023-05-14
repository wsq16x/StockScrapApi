using Microsoft.AspNetCore.Mvc;

namespace StockScrapApi.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
