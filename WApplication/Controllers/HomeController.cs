using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServiceShared.Const;
using ServiceShared.Models;
using System.Text;
using WApplication.Services;

namespace WApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public async Task<string> CallTicketInsert(InsertDataRequestModel input)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");
                string data = await _apiService.PostExternalDataAsync(TicketsConst.urlInsertTicket, content);
                return data;
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }

    }
}
