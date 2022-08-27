using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Components {
	public class RedirectToLoginPage : ComponentBase {
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		protected override void OnInitialized() {
			NavigationManager.NavigateTo("/account/login");
		}
	}
}
