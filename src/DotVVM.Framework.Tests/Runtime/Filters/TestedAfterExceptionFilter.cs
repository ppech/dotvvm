using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters.PresenterFilters;

namespace DotVVM.Framework.Tests.Runtime.Filters
{
	public class TestedAfterExceptionFilter : PresenterFilterAttribute
	{
		protected internal override void AfterProcessing(IDotvvmRequestContext context)
		{
			throw new FilterException(FilterException.TextAfter);
		}
	}
}