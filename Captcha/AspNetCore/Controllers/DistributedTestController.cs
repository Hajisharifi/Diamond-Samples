using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using Diamond.AspNetCore.Html.Captcha;

namespace H2.ASP_Captcha.Controllers
{
    public class DistributedTestController
        : Controller
    {
        //________________________________________________________________________

        private readonly IDistributedCaptcha m_Captcha;
        //________________________________________________________________________

        public DistributedTestController(IDistributedCaptcha captcha)
        {
            this.m_Captcha = captcha;
        }
        //________________________________________________________________________

        public IActionResult Index()
        {
            //var model = new { CaptchaRefID = this.m_Captcha.NewRefID() };
            return this.View(/*model*/);
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

            //var model = new { CaptchaRefID = this.m_Captcha.NewRefID() };
            return this.View(/*model*/);
        }
        //________________________________________________________________________

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string CaptchaRefresh()
        {
            return this.m_Captcha.NewRefID();
        }
        //________________________________________________________________________

        public IActionResult CaptchaImage(string id)
        {
            return this.m_Captcha.GetImage(id);
        }
        //________________________________________________________________________
    }
}
