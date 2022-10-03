using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using FP = Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

using Diamond.Addons;
using AspCoreAddons;
using AspCoreAddons.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DiamondAddonsAspNetCore
    {
        //________________________________________________________________________

        public static RequestDelegate Convert(Func<HttpContext, Task> func)
        {
            return context => func(context);
        }
        public static Func<HttpContext, Task> Convert(RequestDelegate func)
        {
            return context => func(context);
        }
        public static Func<RequestDelegate, RequestDelegate> Convert(Func<Func<HttpContext, Task>, Func<HttpContext, Task>> middleware)
        {
            return next => Convert(middleware(Convert(next)));
        }
        public static Func<Func<HttpContext, Task>, Func<HttpContext, Task>> Convert(Func<RequestDelegate, RequestDelegate> middleware)
        {
            return next => Convert(middleware(Convert(next)));
        }
        public static IApplicationBuilder Use(this IApplicationBuilder app, IMiddleware<HttpContext> middleware)
        {
            return app.Use(Convert(middleware.Use));
        }
        //________________________________________________________________________

        public static void ConfigureAddonsUI(this IMvcBuilder builder, AddonStartupBuilder addIns)
        {
            builder.ConfigureApplicationPartManager(apm =>
            {
                var x = addIns.GetService<IAddonGroup<Microsoft.AspNetCore.Mvc.ControllerBase>>();
                var y = addIns.GetService<IAddonGroup<Microsoft.AspNetCore.Mvc.RazorPages.PageModel>>();
                var z = addIns.GetService<IAddonGroup<Microsoft.AspNetCore.Mvc.Razor.IRazorPage>>();
                
                foreach (var assembly in x.Select(i => i.Addon.Assembly).Union(y.Select(i => i.Addon.Assembly)).Distinct())
                    apm.ApplicationParts.Add(new AssemblyPart(assembly));

                foreach (var assembly in z.Select(i => i.Addon.Assembly).Distinct())
                    apm.ApplicationParts.Add(new CompiledRazorAssemblyPart(assembly));
            });
        }
        //________________________________________________________________________

        public static void UseAddonsStaticFiles(this IApplicationBuilder app)
        {
            var allProviders = app
                .ApplicationServices
                .GetService<IEnumerable<IAddonMetaData>>()
                .Select(a => a.Features.Get<IAddonPath>()?.RealDirectoryPath)
                .Where(path => !string.IsNullOrEmpty(path))
                .Distinct()
                .Select(path => System.IO.Path.Combine(path, "wwwroot"))
                .Where(System.IO.Directory.Exists)
                .Select(path => (FP.IFileProvider)(new PhysicalFileProvider(path)))
                .ToList();

            if (allProviders.Count <= 0)
            {
                app.UseStaticFiles();
            }
            else
            {
                var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
                allProviders.Insert(0, env.WebRootFileProvider);
                env.WebRootFileProvider = new CompositeFileProvider(allProviders);

                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = env.WebRootFileProvider
                });
            }
        }
        //________________________________________________________________________

        public static void UseAddonsMiddlewares(this IApplicationBuilder app)
        {
            //run all IMiddleware
            app.Use(app.ApplicationServices.MergeMiddlewares<HttpContext>());
        }
        //________________________________________________________________________
    }

    public sealed class UrlParameterVisitor
        : Diamond.Addons.IAddonVisitor
    {
        public IEnumerable<IAddonMetaData> Visit(IEnumerable<IAddonMetaData> addIns)
        {
            foreach (var addIn in addIns)
            {
                string part;
                if (addIn.Interface == typeof(Microsoft.AspNetCore.Mvc.ControllerBase))
                {
                    part = addIn.Addon.Name;
                    if (part.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                    {
                        if (part.Length <= 10) continue;
                        part = part.Substring(0, part.Length - 10);
                    }
                }
                else if (addIn.Interface == typeof(Microsoft.AspNetCore.Mvc.RazorPages.PageModel))
                {
                    part = addIn.Addon.Name;
                    if (part.EndsWith("Model", StringComparison.OrdinalIgnoreCase))
                    {
                        if (part.Length <= 5) continue;
                        part = part.Substring(0, part.Length - 5);
                    }
                }
                else
                {
                    continue;
                }
                string area = addIn.Features.Get<Microsoft.AspNetCore.Mvc.AreaAttribute>()?.RouteValue;
                addIn.Features.Set(new UrlParameter(area, part));
            }
            return addIns;
        }
    }
    //________________________________________________________________________

    public sealed class UrlParameter
    {
        public string Area { get; }
        public string Part { get; }
        public UrlParameter(string area, string part)
        {
            this.Area = area;
            this.Part = part;
        }
    }
    //________________________________________________________________________
}
