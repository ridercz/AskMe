using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages
{
    /// <summary>
	/// Layer Supertype (design-pattern) for Blazor components serving pages (responding to routes).
	/// </summary>
	public class PageBase : ComponentBase
    {
		[Inject]
		protected IJsHelpers JsHelpers { get; set; }

		protected string Title { get; set; }

		protected override async Task OnAfterRenderAsync(bool firstRender) {
			await base.OnAfterRenderAsync(firstRender);

			await JsHelpers.SetPageTitleAsync(Title);
		}
	}
}
