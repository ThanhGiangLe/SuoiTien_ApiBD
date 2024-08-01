using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class GgMapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
