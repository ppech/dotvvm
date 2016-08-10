using System;
using DotVVM.Framework.Hosting;

namespace DotVVM.Framework.Runtime.Filters.PresenterFilters
{
    public class PresenterFilterAttribute : Attribute
    {
        /// <summary>
        /// Apply filter before route start request processing.
        /// </summary>
        protected internal virtual void BeforeProcessing(IDotvvmRequestContext context)
        { }

        /// <summary>
        /// Apply filter after route finish request processing.
        /// </summary>
        protected internal virtual void AfterProcessing(IDotvvmRequestContext context)
        { }
    }
}