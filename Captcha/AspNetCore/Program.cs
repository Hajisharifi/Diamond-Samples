using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace H2.ASP_Captcha
{
    public static class Program
    {
        //________________________________________________________________________

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        //________________________________________________________________________

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
        //________________________________________________________________________
    }
}
