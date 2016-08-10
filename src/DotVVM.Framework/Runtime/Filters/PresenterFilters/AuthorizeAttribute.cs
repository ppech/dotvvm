using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DotVVM.Framework.Hosting;
using System.Collections.Concurrent;
using DotVVM.Framework.Runtime.Filters.PresenterFilters.ActionFilters;

namespace DotVVM.Framework.Runtime.Filters.PresenterFilters
{
    /// <summary>
    /// A filter that checks the authorize attributes and redirects to the login page.
    /// </summary>
    public class AuthorizeAttribute : PresenterFilterAttribute
    {

        /// <summary>
        /// Gets or sets the comma-separated list of roles.
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        public AuthorizeAttribute()
        {
        }

        protected internal override void BeforeProcessing(IDotvvmRequestContext context)
        {
            Authorize(context);
            base.BeforeProcessing(context);
        }

        public void Authorize(IDotvvmRequestContext context)
        {
            // check for [NotAuthorized] attribute
            if (context.ViewModel != null && !CanBeAuthorized(context.ViewModel.GetType())) return;

            // the user must not be anonymous
            if (context.OwinContext.Request.User == null || !context.OwinContext.Request.User.Identity.IsAuthenticated)
            {
                SetUnauthorizedResponse(context);
            }

            // if the role is set
            if (Roles != null && Roles.Length > 0)
            {
                if (!Roles.Any(r => context.OwinContext.Request.User.IsInRole(r)))
                {
                    SetUnauthorizedResponse(context);
                }
            }
        }

        private static ConcurrentDictionary<Type, bool> canBeAuthorizedCache = new ConcurrentDictionary<Type, bool>();
        protected static bool CanBeAuthorized(Type viewModelType)
        {
            return canBeAuthorizedCache.GetOrAdd(viewModelType, t => !IsDefined(t, typeof(NotAuthorizedAttribute)));
        }

        protected virtual void SetUnauthorizedResponse(IDotvvmRequestContext context)
        {
            throw new UnauthorizedAccessException();
        }
    }
}