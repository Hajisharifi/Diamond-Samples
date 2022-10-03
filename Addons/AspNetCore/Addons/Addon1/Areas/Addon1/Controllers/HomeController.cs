using System;
using Microsoft.AspNetCore.Mvc;

namespace Addon1.Areas.Addon1.Controllers
{
    [Area("Addon1")]
    public class HomeController
        : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
