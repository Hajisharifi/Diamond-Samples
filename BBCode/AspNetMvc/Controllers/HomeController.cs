using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace AspNetMvc.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        public ActionResult Index()
        {
            return this.View();
        }
        //________________________________________________________________________

        [HttpPost]
        public ActionResult Index(string source)
        {
            IHtmlString model = BBCodeParser.Current.ToHtml(source);
            return this.View(model);
        }
        //________________________________________________________________________
    }
}