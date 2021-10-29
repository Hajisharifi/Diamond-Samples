using System;
using Diamond.FileStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace H2.Core_FileStorage
{
    public class Startup
    {
        //________________________________________________________________________

        private readonly IHostEnvironment m_Environment;
        private readonly IConfiguration m_Configuration;
        //________________________________________________________________________

        public Startup(IHostEnvironment environment, IConfiguration configuration)
        {
            this.m_Environment = environment;
            this.m_Configuration = configuration;
        }
        //________________________________________________________________________

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<Models.Context>(
                options => options.UseSqlServer(this.m_Configuration.GetConnectionString("Default")),
                ServiceLifetime.Scoped,
                ServiceLifetime.Singleton);

            services.AddSingleton<IFileInterceptor<long>, Models.DatabaseProvider>();
            services.AddFileStorage<long>(options =>
            {
                options.Path = @"~\Data\FileStorage\";
                //options.FilePathFactory = new Diamond.FileStorage.Infrastructure.V2Int64DecimalFilePathFactory(); //Support old FileStorage version
            });
        }
        //________________________________________________________________________

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        //________________________________________________________________________
    }
}
