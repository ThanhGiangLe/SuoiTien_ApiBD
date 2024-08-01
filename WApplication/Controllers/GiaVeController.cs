using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GiaVeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
