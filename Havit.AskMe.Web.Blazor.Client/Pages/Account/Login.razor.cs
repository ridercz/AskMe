using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Components;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages.Account
{
    public class LoginBase : PageBase
    {
		[Inject]
		public IAuthenticationClientFacade AuthenticationClientFacade { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		protected LoginIM InputModel { get; set; } = new LoginIM();

		protected ServerSideValidator ServerSideValidator { get; set; }

		public async Task HandleValidSubmit()
		{
			var result = await AuthenticationClientFacade.Login(InputModel);
			if (result.Successful)
			{
				NavigationManager.NavigateTo("/");
			}
			else
			{
				ServerSideValidator.AddError(result.Error);
			}
		}
    }
}
