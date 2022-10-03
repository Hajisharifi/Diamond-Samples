using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using AspCoreAddons.Models;

namespace AspCoreAddons.Controllers
{
    public class HomeController
        : Controller
    {
        private readonly ILogger<HomeController> m_Logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.m_Logger = logger;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
