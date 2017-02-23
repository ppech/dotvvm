using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Hosting;

namespace DotVVM.Framework.Controls
{
	/// <summary>
	///     Renders the pagination control which can be integrated with the GridViewDataSet object to provide the paging
	///     capabilities.
	/// </summary>
	[ControlMarkupOptions(AllowContent = false)]
	public class DataPager : HtmlGenericControl
	{
		private static readonly CommandBindingExpression GoToNextPageCommand =
			new CommandBindingExpression(h => ((IGridViewDataSet) h[0]).GoToNextPage(), "__$DataPager_GoToNextPage");

		private static readonly CommandBindingExpression GoToThisPageCommand =
			new CommandBindingExpression(h => ((IGridViewDataSet) h[1]).GoToPage((int) h[0]), "__$DataPager_GoToThisPage");

		private static readonly CommandBindingExpression GoToPrevPageCommand =
			new CommandBindingExpression(h => ((IGridViewDataSet) h[0]).GoToPreviousPage(), "__$DataPager_GoToPrevPage");

		private static readonly CommandBindingExpression GoToFirstPageCommand =
			new CommandBindingExpression(h => ((IGridViewDataSet) h[0]).GoToFirstPage(), "__$DataPager_GoToFirstPage");

		private static readonly CommandBindingExpression GoToLastPageCommand =
			new CommandBindingExpression(h => ((IGridViewDataSet) h[0]).GoToLastPage(), "__$DataPager_GoToLastPage");

		public static readonly DotvvmProperty DataSetProperty =
			DotvvmProperty.Register<IGridViewDataSet, DataPager>(c => c.DataSet);

		public static readonly DotvvmProperty FirstPageTemplateProperty =
			DotvvmProperty.Register<ITemplate, DataPager>(c => c.FirstPageTemplate, null);

		public static readonly DotvvmProperty LastPageTemplateProperty =
			DotvvmProperty.Register<ITemplate, DataPager>(c => c.LastPageTemplate, null);

		public static readonly DotvvmProperty PreviousPageTemplateProperty =
			DotvvmProperty.Register<ITemplate, DataPager>(c => c.PreviousPageTemplate, null);

		public static readonly DotvvmProperty NextPageTemplateProperty =
			DotvvmProperty.Register<ITemplate, DataPager>(c => c.NextPageTemplate, null);

		public static readonly DotvvmProperty RenderLinkForCurrentPageProperty =
			DotvvmProperty.Register<bool, DataPager>(c => c.RenderLinkForCurrentPage);

		public static readonly DotvvmProperty HideWhenOnlyOnePageProperty
			= DotvvmProperty.Register<bool, DataPager>(c => c.HideWhenOnlyOnePage, true);

		public static readonly DotvvmProperty EnabledProperty
			= DotvvmProperty.Register<bool, DataPager>(c => c.Enabled, true);


		private HtmlGenericControl content;
		private HtmlGenericControl firstLi;
		private HtmlGenericControl lastLi;
		private HtmlGenericControl nextLi;
		private PlaceHolder numbersPlaceHolder;
		private HtmlGenericControl previousLi;


		public DataPager() : base("div")
		{
		}


		/// <summary>
		///     Gets or sets the GridViewDataSet object in the viewmodel.
		/// </summary>
		[MarkupOptions(AllowHardCodedValue = false)]
		public IGridViewDataSet DataSet
		{
			get { return (IGridViewDataSet) GetValue(DataSetProperty); }
			set { SetValue(DataSetProperty, value); }
		}


		/// <summary>
		///     Gets or sets the template of the button which moves the user to the first page.
		/// </summary>
		[MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
		public ITemplate FirstPageTemplate
		{
			get { return (ITemplate) GetValue(FirstPageTemplateProperty); }
			set { SetValue(FirstPageTemplateProperty, value); }
		}

		/// <summary>
		///     Gets or sets the template of the button which moves the user to the last page.
		/// </summary>
		[MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
		public ITemplate LastPageTemplate
		{
			get { return (ITemplate) GetValue(LastPageTemplateProperty); }
			set { SetValue(LastPageTemplateProperty, value); }
		}

		/// <summary>
		///     Gets or sets the template of the button which moves the user to the previous page.
		/// </summary>
		[MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
		public ITemplate PreviousPageTemplate
		{
			get { return (ITemplate) GetValue(PreviousPageTemplateProperty); }
			set { SetValue(PreviousPageTemplateProperty, value); }
		}

		/// <summary>
		///     Gets or sets the template of the button which moves the user to the next page.
		/// </summary>
		[MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
		public ITemplate NextPageTemplate
		{
			get { return (ITemplate) GetValue(NextPageTemplateProperty); }
			set { SetValue(NextPageTemplateProperty, value); }
		}

		/// <summary>
		///     Gets or sets whether a hyperlink should be rendered for the current page number. If set to false, only a plain text
		///     is rendered.
		/// </summary>
		[MarkupOptions(AllowBinding = false)]
		public bool RenderLinkForCurrentPage
		{
			get { return (bool) GetValue(RenderLinkForCurrentPageProperty); }
			set { SetValue(RenderLinkForCurrentPageProperty, value); }
		}


		/// <summary>
		///     Gets or sets whether the pager should hide automatically when there is only one page of results. Must not be set to
		///     true when using the Visible property.
		/// </summary>
		[MarkupOptions(AllowBinding = false)]
		public bool HideWhenOnlyOnePage
		{
			get { return (bool) GetValue(HideWhenOnlyOnePageProperty); }
			set { SetValue(HideWhenOnlyOnePageProperty, value); }
		}

		public bool Enabled
		{
			get { return (bool) GetValue(EnabledProperty); }
			set { SetValue(EnabledProperty, value); }
		}

		protected internal override void OnLoad(IDotvvmRequestContext context)
		{
			DataBind(context);
			base.OnLoad(context);
		}

		protected internal override void OnPreRender(IDotvvmRequestContext context)
		{
			DataBind(context);
			base.OnPreRender(context);
		}

		private void DataBind(IDotvvmRequestContext context)
		{
			Children.Clear();

			content = new HtmlGenericControl("ul");
			content.SetBinding(DataContextProperty, GetDataSetBinding());
			Children.Add(content);

			var dataSet = DataSet;
			if (dataSet != null)
			{
				object enabledValue = HasValueBinding(EnabledProperty)
					? (object)new ValueBindingExpression(h => GetValueBinding(EnabledProperty).Evaluate(this, EnabledProperty), "$pagerEnabled")
					: Enabled;

				// first button
				firstLi = new HtmlGenericControl("li");
				var firstLink = new LinkButton();
				SetButtonContent(context, firstLink, "««", FirstPageTemplate);
				firstLink.SetBinding(ButtonBase.ClickProperty, GoToFirstPageCommand);
				if (!true.Equals(enabledValue))
				{
					firstLink.SetValue(ButtonBase.EnabledProperty, enabledValue);
				}
				firstLi.Children.Add(firstLink);
				content.Children.Add(firstLi);

				// previous button
				previousLi = new HtmlGenericControl("li");
				var previousLink = new LinkButton();
				SetButtonContent(context, previousLink, "«", PreviousPageTemplate);
				previousLink.SetBinding(ButtonBase.ClickProperty, GoToPrevPageCommand);
				if (!true.Equals(enabledValue))
				{
					previousLink.SetValue(ButtonBase.EnabledProperty, enabledValue);
				}
				previousLi.Children.Add(previousLink);
				content.Children.Add(previousLi);

				// number fields
				numbersPlaceHolder = new PlaceHolder();
				content.Children.Add(numbersPlaceHolder);

				var i = 0;
				foreach (var number in dataSet.NearPageNumbers)
				{
					var li = new HtmlGenericControl("li");
					li.SetBinding(DataContextProperty, GetNearIndexesBinding(i));
					if (number == dataSet.PageNumber)
					{
						li.Attributes["class"] = "active";
					}
					var link = new LinkButton
					{
						Text = number.ToString()
					};
					link.SetBinding(ButtonBase.ClickProperty, GoToThisPageCommand);
					if (!true.Equals(enabledValue))
					{
						link.SetValue(ButtonBase.EnabledProperty, enabledValue);
					}
					li.Children.Add(link);
					numbersPlaceHolder.Children.Add(li);

					i++;
				}

				// next button
				nextLi = new HtmlGenericControl("li");
				var nextLink = new LinkButton();
				SetButtonContent(context, nextLink, "»", NextPageTemplate);
				nextLink.SetBinding(ButtonBase.ClickProperty, GoToNextPageCommand);
				if (!true.Equals(enabledValue))
				{
					nextLink.SetValue(ButtonBase.EnabledProperty, enabledValue);
				}
				nextLi.Children.Add(nextLink);
				content.Children.Add(nextLi);

				// last button
				lastLi = new HtmlGenericControl("li");
				var lastLink = new LinkButton();
				SetButtonContent(context, lastLink, "»»", LastPageTemplate);
				if (!true.Equals(enabledValue))
				{
					lastLink.SetValue(ButtonBase.EnabledProperty, enabledValue);
				}
				lastLink.SetBinding(ButtonBase.ClickProperty, GoToLastPageCommand);
				lastLi.Children.Add(lastLink);
				content.Children.Add(lastLi);
			}
		}

		private void SetButtonContent(IDotvvmRequestContext context, LinkButton button, string text, ITemplate contentTemplate)
		{
			if (contentTemplate != null)
			{
				contentTemplate.BuildContent(context, button);
			}
			else
			{
				button.Text = text;
			}
		}

		private ValueBindingExpression GetNearIndexesBinding(int pageIndex)
		{
			return new ValueBindingExpression(
				(h, c) => ((IGridViewDataSet) h[0]).NearPageNumbers[pageIndex],
				"NearPageNumbers[" + pageIndex + "]");
		}

		protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
		{
			if (RenderOnServer)
			{
				throw new DotvvmControlException(this,
					"The DataPager control cannot be rendered in the RenderSettings.Mode='Server'.");
			}

			base.AddAttributesToRender(writer, context);
		}

		protected override void AddVisibleAttributeOrBinding(IHtmlWriter writer)
		{
			if (!IsPropertySet(VisibleProperty))
			{
				if (HideWhenOnlyOnePage)
				{
					writer.AddKnockoutDataBind("visible",
						$"ko.unwrap({GetDataSetBinding().GetKnockoutBindingExpression()}).PagesCount() > 1");
				}
				else
				{
					writer.AddKnockoutDataBind("visible", this, VisibleProperty, renderEvenInServerRenderingMode: true);
				}
			}
		}

		protected override void RenderBeginTag(IHtmlWriter writer, IDotvvmRequestContext context)
		{
			if (HasValueBinding(EnabledProperty))
			{
				writer.WriteKnockoutDataBindComment("dotvvm_introduceAlias",
					$"{{ '$pagerEnabled': {GetValueBinding(EnabledProperty).GetKnockoutBindingExpression()}}}");
			}

			writer.AddKnockoutDataBind("with", this, DataSetProperty, renderEvenInServerRenderingMode: true);
			writer.RenderBeginTag("ul");
		}

		protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
		{
			writer.AddKnockoutDataBind("css", "{ 'disabled': IsFirstPage() }");
			firstLi.Render(writer, context);

			writer.AddKnockoutDataBind("css", "{ 'disabled': IsFirstPage() }");
			previousLi.Render(writer, context);

			// render template
			writer.WriteKnockoutForeachComment("NearPageNumbers");

			// render page number
			numbersPlaceHolder.Children.Clear();
			HtmlGenericControl li;
			if (!RenderLinkForCurrentPage)
			{
				writer.AddKnockoutDataBind("visible", "$data == $parent.PageNumber()");
				writer.AddKnockoutDataBind("css", "{'active': $data == $parent.PageNumber()}");
				li = new HtmlGenericControl("li");
				var literal = new Literal();
				literal.DataContext = 0;
				literal.SetBinding(Literal.TextProperty, new ValueBindingExpression(vm => ((int) vm[0]).ToString(), "$data"));
				li.Children.Add(literal);
				numbersPlaceHolder.Children.Add(li);
				li.Render(writer, context);

				writer.AddKnockoutDataBind("visible", "$data != $parent.PageNumber()");
			}
			writer.AddKnockoutDataBind("css", "{ 'active': $data == $parent.PageNumber()}");
			li = new HtmlGenericControl("li");
			li.SetValue(Internal.PathFragmentProperty, "NearPageNumbers[$index]");
			var link = new LinkButton();
			li.Children.Add(link);
			link.SetBinding(ButtonBase.TextProperty, new ValueBindingExpression(vm => ((int) vm[0]).ToString(), "$data"));
			link.SetBinding(ButtonBase.ClickProperty, GoToThisPageCommand);
			var enabledValue = HasValueBinding(EnabledProperty)
				? (object)new ValueBindingExpression(h => GetValueBinding(EnabledProperty).Evaluate(this, EnabledProperty), "$pagerEnabled")
				: Enabled;
			if (!true.Equals(enabledValue))
			{
				link.SetValue(ButtonBase.EnabledProperty, enabledValue);
			}
			numbersPlaceHolder.Children.Add(li);
			li.Render(writer, context);

			writer.WriteKnockoutDataBindEndComment();

			writer.AddKnockoutDataBind("css", "{ 'disabled': IsLastPage() }");
			nextLi.Render(writer, context);

			writer.AddKnockoutDataBind("css", "{ 'disabled': IsLastPage() }");
			lastLi.Render(writer, context);
		}


		protected override void RenderEndTag(IHtmlWriter writer, IDotvvmRequestContext context)
		{
			writer.RenderEndTag();
			if (HasValueBinding(EnabledProperty))
			{
				writer.WriteKnockoutDataBindEndComment();
			}
		}

		private IValueBinding GetDataSetBinding()
		{
			var binding = GetValueBinding(DataSetProperty);
			if (binding == null)
			{
				throw new DotvvmControlException(this, "The DataSet property of the dot:DataPager control must be set!");
			}
			return binding;
		}
	}
}