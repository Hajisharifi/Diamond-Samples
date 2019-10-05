using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace AspNetCore.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        private readonly IBBCodeParser m_BBCodeParser;
        //________________________________________________________________________

        public HomeController(IBBCodeParser parser)
        {
            this.m_BBCodeParser = parser;
        }
        //________________________________________________________________________

        public IActionResult Index()
        {
            return this.View();
        }
        //________________________________________________________________________

        [HttpPost]
        public IActionResult Index(string source)
        {
            IHtmlContent model = this.m_BBCodeParser.ToHtml(source);
            return this.View(model);
        }
        //________________________________________________________________________    }
    }
}
