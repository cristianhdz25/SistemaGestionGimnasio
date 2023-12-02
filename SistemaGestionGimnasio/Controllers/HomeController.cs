using Microsoft.AspNetCore.Mvc;
using SistemaGestionGimnasio.Models;
using System.Diagnostics;

namespace SistemaGestionGimnasio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult IndexAdmin()
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            return View();
        }

        public IActionResult IndexEmpleado()
        {
            if (HttpContext.Session.GetString("Usuario") == "" || HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Usuario");
            }
            return View();
        }

        public IActionResult Privacy()
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