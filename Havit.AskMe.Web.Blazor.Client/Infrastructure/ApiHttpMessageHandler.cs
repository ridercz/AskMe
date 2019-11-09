using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Infrastructure {
	public class ApiHttpMessageHandler : DelegatingHandler {
		private readonly NavigationManager navigationManager;
		private readonly IApiAuthenticationStateProvider apiAuthenticationStateProvider;

		public ApiHttpMessageHandler(
			HttpMessageHandler innerHandler,
			NavigationManager navigationManager,
			IApiAuthenticationStateProvider apiAuthenticationStateProvider)
			: base(innerHandler) {
			this.navigationManager = navigationManager;
			this.apiAuthenticationStateProvider = apiAuthenticationStateProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
			var token = await apiAuthenticationStateProvider.GetTokenAsync();

			if (!string.IsNullOrWhiteSpace(token)) {
				request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
			}

			var response = await base.SendAsync(request, cancellationToken);

			switch (response.StatusCode) {
				case HttpStatusCode.OK:
					// NOOP
					break;
				case HttpStatusCode.Unauthorized:
					navigationManager.NavigateTo("/account/login");
					break;
				case HttpStatusCode.NotFound:
					navigationManager.NavigateTo("/error/404");
					break;
				case HttpStatusCode.InternalServerError:
					navigationManager.NavigateTo("/error/500");
					break;
				default:
					navigationManager.NavigateTo($"/error/{response.StatusCode:d}");
					break;
			}

			return response;
		}
	}
}
