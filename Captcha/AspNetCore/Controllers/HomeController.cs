using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;

using Diamond.AspNetCore.Html.Captcha;

namespace H2.ASP_Captcha.Controllers
{
    public class HomeController
        : Controller
    {
        //________________________________________________________________________

        private readonly ICachedCaptcha m_Captcha;
        //________________________________________________________________________

        public HomeController(ICachedCaptcha captcha)
        {
            this.m_Captcha = captcha;
        }
        //________________________________________________________________________

        public IActionResult Index()
        {
            return this.View();
        }
        //________________________________________________________________________

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string Captcha_RefID, string Input_Captcha)
        {
            bool isValid = this.m_Captcha.Validate(Captcha_RefID, Input_Captcha);

            if (isValid)
            {
                this.ViewBag.Message = new HtmlString(@"<span class=""captcha-ok"">OK</span>");
            }
            else
            {
                this.ViewBag.Message = new HtmlString(@"<span class=""captcha-error"">ERROR</span>");
            }

            return this.View();
        }
        //________________________________________________________________________

        public IActionResult CaptchaImage(string id, int version = 0)
        {
            return this.m_Captcha.GetImage(id, version);
        }
        //________________________________________________________________________
    }
}
