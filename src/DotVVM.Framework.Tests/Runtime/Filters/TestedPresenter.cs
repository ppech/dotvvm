using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime;
using DotVVM.Framework.Security;
using DotVVM.Framework.ViewModel.Serialization;

namespace DotVVM.Framework.Tests.Runtime.Filters
{
    public class TestedPresenter : DotvvmPresenter
    {
        public TestedPresenter(DotvvmConfiguration configuration) : base(configuration)
        {
        }

        public TestedPresenter(IDotvvmViewBuilder dotvvmViewBuilder, IViewModelLoader viewModelLoader, IViewModelSerializer viewModelSerializer, IOutputRenderer outputRenderer, ICsrfProtector csrfProtector) : base(dotvvmViewBuilder, viewModelLoader, viewModelSerializer, outputRenderer, csrfProtector)
        {
        }
    }
}