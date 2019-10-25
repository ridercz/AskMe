using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages.Account
{
    public class LogoutBase : PageBase
    {
		[Inject]
		public IApiAuthenticationStateProvider ApiAuthenticationStateProvider { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			await ApiAuthenticationStateProvider.SetAuthenticatedUser(null);
		}
	}
}
