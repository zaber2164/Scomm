using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Scomm.Models;
using System.Diagnostics;

namespace Scomm.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Item()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Category()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}