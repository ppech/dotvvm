using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotVVM.Framework.Hosting
{
    public class DotvvmRerouteException : Exception
    {
        public DotvvmRerouteException(string routeName, object routeValues)
            : base($"Request has been rerouted to the '{routeName}' route.")
        {
            RouteName = routeName;
            RouteValues = routeValues;
        }

        public string RouteName { get; }

        public object RouteValues { get; }
    }
}
