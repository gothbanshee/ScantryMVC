using Microsoft.AspNetCore.Mvc;
using Scantry.Models;
using System.Diagnostics;
using static Scantry.GCommon.GeneralMessages;

namespace Scantry.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            TempData["ErrorMessage"] = GeneralError;
            return RedirectToAction("Index", "Home");
        }
    }
}
