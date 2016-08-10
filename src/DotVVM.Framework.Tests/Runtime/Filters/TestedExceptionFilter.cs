using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters.PresenterFilters;

namespace DotVVM.Framework.Tests.Runtime.Filters
{
    public class TestedExceptionFilter : PresenterFilterAttribute
    {
	    public bool ExceptionBefore { get; }
	    public bool ExpectionAfter { get; }

	    public TestedExceptionFilter(bool exceptionBefore, bool expectionAfter)
	    {
		    ExceptionBefore = exceptionBefore;
		    ExpectionAfter = expectionAfter;
	    }

	    protected internal override void BeforeProcessing(IDotvvmRequestContext context)
        {
		    if (ExceptionBefore)
		    {
			    throw new FilterException(FilterException.TextBefore);
		    }
        }

	    protected internal override void AfterProcessing(IDotvvmRequestContext context)
	    {
		    if (ExpectionAfter)
		    {
			    throw new FilterException(FilterException.TextAfter);
		    }
	    }
    }
}