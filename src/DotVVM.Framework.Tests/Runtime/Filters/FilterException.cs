using System;

namespace DotVVM.Framework.Tests.Runtime.Filters
{
	public class FilterException : Exception
	{
		public const string TextBefore = "before";
		public const string TextAfter = "after";

		public FilterException(string message) : base(message)
		{
		}
	}
}