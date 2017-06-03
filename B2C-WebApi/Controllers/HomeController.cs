using Microsoft.AspNetCore.Mvc;

namespace B2C_WebApi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}