using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Diamond.Addons;

namespace AspCoreAddons
{
    public class Startup
    {
        //________________________________________________________________________

        private const string AddonsPattern = "Addons\\*\\*.dll";

        private readonly IHostEnvironment m_Environment;
        private readonly IConfiguration m_Configuration;
        private AddonStartupBuilder m_Addons;
        //________________________________________________________________________

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            this.m_Configuration = configuration;
            this.m_Environment = environment;
        }
        //________________________________________________________________________

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //config & scan addins
            this.m_Addons = new AddonStartupBuilder(this.m_Environment.ContentRootPath);
            this.m_Addons.AddFileInclude(AddonsPattern);
            this.m_Addons.AddFileExclude("**\\System.*.dll"); //for-speed
            this.m_Addons.AddFileExclude("**\\Microsoft.*.dll"); //for-speed
            this.m_Addons.AddDefaultVisitors();
            this.m_Addons.AddVisitor<UrlParameterVisitor>();
            this.m_Addons.AddFilterByInherits<IAddon>();
            this.m_Addons.AddFilterByInherits<Microsoft.AspNetCore.Mvc.ControllerBase>();
            this.m_Addons.AddFilterByInherits<Microsoft.AspNetCore.Mvc.RazorPages.PageModel>();
            this.m_Addons.AddFilterByInherits<Microsoft.AspNetCore.Mvc.Razor.IRazorPage>();
            this.m_Addons.EnabledDynamicVisitors = true;
            this.m_Addons.Scan();
            this.m_Addons.ConfigureServices(services);



            services.AddRazorPages();
            services
                .AddControllersWithViews()
                .ConfigureAddonsUI(this.m_Addons);
        }
        //________________________________________________________________________

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //run all IStartup.Configure
            app.ApplicationServices.StartupsConfigure(); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //download static files
            app.UseAddonsStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //run all IMiddleware<HttpContext>
            app.UseAddonsMiddlewares();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //________________________________________________________________________
    }
}
