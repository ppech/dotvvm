﻿using System.Collections.Generic;
using System.Security.Claims;

namespace DotVVM.Framework.Hosting
{
    public interface IHttpContext
    {
        ClaimsPrincipal User { get; set; }
        IHttpRequest Request { get; set; }
        IHttpResponse Response { get; set; }
        IAuthentication Authentication { get; }
        IDictionary<object, object> Items { get; set; }
    }
}