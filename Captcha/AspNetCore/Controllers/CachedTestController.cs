using System;
using Diamond.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;

namespace H2.ASP_Captcha.Controllers;

public class CachedTestController
    : Controller
{
    //________________________________________________________________________

    private readonly ICachedCaptcha m_Captcha;
    //________________________________________________________________________

    public CachedTestController(ICachedCaptcha captcha)
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

    public IActionResult CaptchaImage(string id, int version = 0)
    {
        return this.m_Captcha.GetImage(id, version);
    }
    //________________________________________________________________________
}
