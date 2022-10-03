using System;
using Diamond.Addons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Addon1
{
    public class CustomizeMiddleware
        : IMiddleware<HttpContext>
    {
        public int PriorityOrder => 0;

        public Func<HttpContext, Task> Use(Func<HttpContext, Task> next)
        {
            return next;
        }
    }
}
