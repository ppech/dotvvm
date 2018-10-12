using System.Threading.Tasks;
using Microsoft.Owin;

namespace DotVVM.Framework.Hosting.Middlewares
{
    public class DotvvmRerouteMiddleware : OwinMiddleware
    {
        private readonly OwinMiddleware next;

        public DotvvmRerouteMiddleware(OwinMiddleware next)
            : base(next)
        {
            this.next = next;
        }

        public override async Task Invoke(IOwinContext context)
        {
            while (true)
            {
                try
                {
                    await next.Invoke(context);
                    return;
                }
                catch (DotvvmRerouteException exception)
                {
                    var dotvvmContext = context.GetDotvvmContext();
                    var route = dotvvmContext.Configuration.RouteTable[exception.RouteName];
                    var url = route.BuildUrl(exception.RouteValues).Trim(new[] { '~' });
                    context.Request.Path = new PathString(url);
                }
            }
        }
    }
}
