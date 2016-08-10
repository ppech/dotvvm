using System.Collections.Generic;
using DotVVM.Framework.Runtime.Filters.PresenterFilters;
using Newtonsoft.Json;
using DotVVM.Framework.Runtime.Filters.PresenterFilters.ActionFilters;

namespace DotVVM.Framework.Configuration
{
    public class DotvvmRuntimeConfiguration
    {

        /// <summary>
        /// Gets filters that are applied for all requests.
        /// </summary>
        [JsonIgnore()]
        public List<ActionFilterAttribute> GlobalActionFilters { get; private set; }
        public List<PresenterFilterAttribute> GlobalPresenterFilters { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DotvvmRuntimeConfiguration"/> class.
        /// </summary>
        public DotvvmRuntimeConfiguration()
        {
            GlobalActionFilters = new List<ActionFilterAttribute>();
            GlobalPresenterFilters = new List<PresenterFilterAttribute>();
        }
    }
}