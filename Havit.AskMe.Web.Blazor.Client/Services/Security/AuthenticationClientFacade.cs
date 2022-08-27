using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Infrastructure;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Account;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security {
	public class AuthenticationClientFacade : IAuthenticationClientFacade {
		private readonly HttpClient httpClient;
		private readonly IApiAuthenticationStateProvider apiAuthenticationStateProvider;

		public AuthenticationClientFacade(HttpClient httpClient, IApiAuthenticationStateProvider apiAuthenticationStateProvider) {
			this.httpClient = httpClient;
			this.apiAuthenticationStateProvider = apiAuthenticationStateProvider;
		}

		public async Task<LoginVM> Login(LoginIM inputModel) {
			try {
				var response = await httpClient.PostAsJsonAsync("api/accounts/login", inputModel);
				var result = await response.Content.ReadFromJsonAsync<LoginVM>();

				if (result.Successful) {
					await apiAuthenticationStateProvider.SetAuthenticatedUserAsync(result.Token, inputModel.RememberMe);
				} else {
					await apiAuthenticationStateProvider.SignOutAsync();
				}
				return result;
			}
			catch (Exception ex) {
				Console.WriteLine(ex);
				return null;
			}
		}
	}
}
