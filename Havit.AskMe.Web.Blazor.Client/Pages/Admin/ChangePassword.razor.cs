using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Components;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages.Admin {
	public class ChangePasswordBase : PageBase {
		[Inject]
		public IAccountClientFacade AccountClientFacade { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		protected ChangePasswordIM InputModel { get; set; } = new ChangePasswordIM();

		protected ServerSideValidator ServerSideValidator { get; set; }

		public async Task HandleValidSubmit() {
			var result = await AccountClientFacade.ChangePasswordAsync(InputModel);
			if (result.Succeeded) {
				NavigationManager.NavigateTo("/Admin/ChangePasswordDone");
			} else {
				foreach (var error in result.Errors) {
					ServerSideValidator.AddError(error);
				}
			}
		}
	}
}
