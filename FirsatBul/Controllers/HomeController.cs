using FirsatBul.Business.Abstract;
using FirsatBul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FirsatBul.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IFirsatlarService _firsatlarService;

        public HomeController(ILogger<HomeController> logger, IFirsatlarService firsatlarService)
        {
            _logger = logger;
            _firsatlarService = firsatlarService;
        }
     [Authorize]
        public IActionResult Index()
        {
           var model = _firsatlarService.GetAllFirsatlar();
            ViewBag.currentUser = TempData["LogInUser"];
            TempData.Keep("LogInUser");
            return View(model);
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
