using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages.Account {
	public class LogoutBase : PageBase {
		[Inject]
		public IApiAuthenticationStateProvider ApiAuthenticationStateProvider { get; set; }

		protected override async Task OnInitializedAsync() {
			await base.OnInitializedAsync();

			await ApiAuthenticationStateProvider.SignOutAsync();
		}
	}
}
