using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotVVM.Framework.Hosting.Middlewares
{
    public class DotvvmRerouteMiddleware
    {
        private readonly RequestDelegate next;

        public DotvvmRerouteMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            while(true)
            {
                try
                {
                    await next(context);
                    return;
                }
                catch(DotvvmRerouteException exception)
                {
                    var configuration = context.RequestServices.GetRequiredService<DotvvmConfiguration>();
                    var route = configuration.RouteTable[exception.RouteName];
                    var url = route.BuildUrl(exception.RouteValues);
                    context.Request.Path = new PathString(url);
                }
            }
        }
    }
}
