using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security {
	public class AccountClientFacade : IAccountClientFacade {
		private readonly HttpClient httpClient;

		public AccountClientFacade(HttpClient httpClient) {
			this.httpClient = httpClient;
		}

		public async Task<ChangePasswordVM> ChangePasswordAsync(ChangePasswordIM inputModel) {
			var response = await httpClient.PostAsJsonAsync("api/accounts/changepassword", inputModel);
			return await response.Content.ReadFromJsonAsync<ChangePasswordVM>();
		}
	}
}
