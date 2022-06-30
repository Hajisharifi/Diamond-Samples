using System;
using System.Linq;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;

using Diamond.BBCode;
using Diamond.BBCode.Core;

namespace AspNetCore.Controllers
{
    //________________________________________________________________________

    public interface IBBCodeParser
    {
        IHtmlContent ToHtml(string bbcode, bool replaceEmoticons = true);
        string ToText(string bbcode);
    }
    //________________________________________________________________________

    public class BBCodeParser
        : IBBCodeParser
    {
        //________________________________________________________________________

        private readonly IServiceProvider m_Services;
        private readonly IDictionary<string, ITagModuleCompile> m_HtmlTagModules;
        private readonly IDictionary<string, ITagModuleCompile> m_TextTagModules;
        //________________________________________________________________________

        public BBCodeParser(
            IServiceProvider services,
            IWebHostEnvironment environment)
        {
            this.m_Services = services;
            var rows = ReadDatabase(environment);

            var compiler1 = new CSharpBBCodeCompiler();
            compiler1.DataSource = rows;
            //compiler1.SetDataSource(rows, r => r.TagName, r => r.RazorTemplate, r => r.ValidationCode);
            this.m_HtmlTagModules = compiler1.Compile();

            var compiler2 = new LiteralTextBBCodeCompiler();
            compiler2.DataSource = rows;
            //compiler2.SetDataSource(rows, r => r.TagName, r => r.RazorTemplate, r => r.ValidationCode);
            this.m_TextTagModules = compiler2.Compile();
        }
        //________________________________________________________________________

        private static IList<DataRow> ReadDatabase(IWebHostEnvironment environment)
        {
            //return EF_DB_Context.BBCodes.ToList();

            string filePath = System.IO.Path.Combine(environment.ContentRootPath, "App_Data\\bbcode-database.xml");
            if (!System.IO.File.Exists(filePath)) return null;

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(
                typeof(List<DataRow>),
                new System.Xml.Serialization.XmlRootAttribute("doc"));

            using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                return (List<DataRow>)xmlSerializer.Deserialize(stream);
            }
        }
        //________________________________________________________________________

        private string[] ReadDatabaseEmoticons()
        {
            return new string[] {
                ":)", "<img src='/Content/icon1.png' />",
                ":(", "<img src='/Content/icon2.png' />",
                ":|", "<img src='/Content/icon3.png' />"};
        }
        //________________________________________________________________________

        public IHtmlContent ToHtml(string bbcode, bool replaceEmoticons = true)
        {
            if (string.IsNullOrWhiteSpace(bbcode))
                return HtmlString.Empty;

            var template = new Diamond.BBCode.Template();
            template.Modules = this.m_HtmlTagModules;

            //for use at 'RenderCode' & 'ValidationCode'
            template.ServiceProvider = this.m_Services;
            template.User = null; //Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext.User;

            template.MarkupDocument = bbcode;
            //return template.RenderBody();

            string html;
            using (var writer = new System.IO.StringWriter())
            {
                writer.WriteLine(@"<div class=""bbcode"">");
                template.RenderBody(writer);
                writer.WriteLine();
                writer.WriteLine(@"</div>");
                html = writer.ToString();
            }

            if (replaceEmoticons)
            {
                var emoticons = this.ReadDatabaseEmoticons();
                //html = Diamond.Text.Functions.Replace(html, StringComparison.OrdinalIgnoreCase, emoticons);
                for (int i = 0; i < emoticons.Length; i += 2)
                    html = html.Replace(emoticons[i], emoticons[i + 1]);
            }

            return new HtmlString(html);
        }
        //________________________________________________________________________

        public string ToText(string bbcode)
        {
            if (string.IsNullOrEmpty(bbcode)) return string.Empty;

            var template = new Template();
            template.Modules = this.m_TextTagModules;

            //for use at 'RenderCode' & 'ValidationCode'
            template.ServiceProvider = this.m_Services;
            template.User = null; //Microsoft.AspNetCore.Http.IHttpContextAccessor.HttpContext.User;

            template.EnableHtmlEncode = false;
            template.MarkupDocument = bbcode;

            using (var writer = new System.IO.StringWriter())
            {
                template.RenderBody(writer);
                bbcode = writer.ToString();
            }

            return bbcode;
        }
        //________________________________________________________________________
    }
}