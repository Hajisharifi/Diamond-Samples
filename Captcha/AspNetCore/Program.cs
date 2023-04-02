using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace H2.ASP_Captcha
{
    public static class Program
    {
        //________________________________________________________________________

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            /**************/
            builder.Services.AddCachedCaptcha(options =>
            {
                options.Width = 120;
                options.Height = 30;
                options.WordLength = 5;
                //options.SlidingExpiration = new TimeSpan(0, 15, 0);
            });
            /**************/
            builder.Services.AddDistributedCaptcha(options =>
            {
                options.Width = 120;
                options.Height = 30;
                options.WordLength = 5;
                options.Key = "mY@PaSsWoRd$12345";
                //options.SlidingExpiration = new TimeSpan(0, 15, 0);
            });
            /**************/


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=DistributedTest}/{action=Index}/{id?}");

            app.Run();
        }
        //________________________________________________________________________
    }
}
