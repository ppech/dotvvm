using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace DotVVM.Samples.Common.ViewModels.FeatureSamples.Reroute
{
    public class RerouteViewModel : DotvvmViewModelBase
    {
        public void Reroute()
        {
            Context.Reroute("FeatureSamples_Reroute_RerouteDestination");
        }

        public override Task Load()
        {
            Context.Reroute("FeatureSamples_Reroute_RerouteDestination");
            return base.Load();
        }
    }
}

