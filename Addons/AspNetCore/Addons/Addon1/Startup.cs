using System;
using Diamond.Addons;
using Microsoft.Extensions.DependencyInjection;

namespace Addon1
{
    public class Startup
        : IStartup
    {
        public Startup(IAddonPath path)
        {
        }

        public void Configure(IServiceProvider services)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
