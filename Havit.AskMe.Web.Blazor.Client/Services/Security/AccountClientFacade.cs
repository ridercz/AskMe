using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security
{
	public class AccountClientFacade : IAccountClientFacade
	{
		private readonly HttpClient httpClient;

		public AccountClientFacade(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public Task<ChangePasswordVM> ChangePasswordAsync(ChangePasswordIM inputModel)
		{
			return httpClient.PostJsonAsync<ChangePasswordVM>("api/accounts/changepassword", inputModel);
		}
	}
}
