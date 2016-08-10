using System.Data;
using System.Diagnostics;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Routing;
using DotVVM.Framework.Runtime;
using DotVVM.Framework.Security;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DotVVM.Framework.Tests.Runtime.Filters
{
    [TestClass]
    public class PresenterFilterTest
    {
		[TestMethod]
		public void TestBeforeException()
        {
	        TestException(FilterException.TextBefore, new TestedExceptionFilter(true, false));
        }

		[TestMethod]
		public void TestAfterException()
		{
			TestException(FilterException.TextAfter, new TestedExceptionFilter(false, true));
		}

		public void TestException(string expectedMessage, TestedExceptionFilter filter)
	    {
			var mockPresenter = new Mock<IDotvvmPresenter>();
			var route = new DotvvmRoute("foo", "", null, () => mockPresenter.Object);

			var configuration = new DotvvmConfiguration();
			configuration.Runtime.GlobalPresenterFilters.Add(filter);
			var context = new DotvvmRequestContext() { Configuration = configuration };
			try
			{
				route.ProcessRequest(context);
				Assert.Fail("Request process passed without exception.");
			}
			catch (FilterException e)
			{
				Assert.AreEqual(expectedMessage, e.Message);
			}
		}
    }
}