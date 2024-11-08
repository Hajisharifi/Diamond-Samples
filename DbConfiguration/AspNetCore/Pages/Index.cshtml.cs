using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspCoreDbConfig.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> m_Logger;
        private readonly IConfiguration m_Configuration;

        public Model MySettings { get; } = new Model();

        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            this.m_Logger = logger;
            this.m_Configuration = configuration;

            this.MySettings.BBB = this.m_Configuration["AAA:BBB"];
            this.MySettings.CCC = this.m_Configuration.GetSection("AAA:CCC").GetChildren().Select(c => int.Parse(c.Value!)).ToArray();
            //this.m_Configuration.GetSection("AAA").Bind(this.MySettings);
        }

        public void OnGet()
        {
        }
    }

    public class Model
    {
        public string? BBB { get; set; }
        public int[]? CCC { get; set; }
    }
}