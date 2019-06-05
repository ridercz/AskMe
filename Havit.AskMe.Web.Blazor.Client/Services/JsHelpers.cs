using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
	public class JsHelpers : IJsHelpers
	{
		private readonly IJSRuntime runtime;

		public JsHelpers(IJSRuntime runtime)
		{
			this.runtime = runtime;
		}

		public Task SetElementAttributeAsync(string elementId, string attributeName, string attributeValue)
		{	
			return runtime.InvokeAsync<object>("setElementAttributeById", elementId, attributeName, attributeValue);
		}

		public Task SetElementAttributeAsync(ElementRef elementRef, string attributeName, string attributeValue)
		{
			return runtime.InvokeAsync<object>("setElementAttribute", elementRef, attributeName, attributeValue);
		}

		public Task SetPageTitleAsync(string title)
		{
			var titleToSet = String.IsNullOrWhiteSpace(title) ? "ASKme" : (title + " | ASKme");
			return runtime.InvokeAsync<object>("setPageTitle", titleToSet);
		}
	}
}
