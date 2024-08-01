using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class HoatDongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SuKien()
        {
            return View();
        }
    }
}
