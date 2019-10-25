using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services.Security
{
	public class AuthenticationDelegatingHandler : DelegatingHandler
	{
		private readonly NavigationManager navigationManager;
		private readonly IApiAuthenticationStateProvider apiAuthenticationStateProvider;

		public AuthenticationDelegatingHandler(HttpMessageHandler innerHandler, NavigationManager navigationManager, IApiAuthenticationStateProvider apiAuthenticationStateProvider)
			: base(innerHandler)
		{
			this.navigationManager = navigationManager;
			this.apiAuthenticationStateProvider = apiAuthenticationStateProvider;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var token = await apiAuthenticationStateProvider.GetToken();

			if (!String.IsNullOrWhiteSpace(token))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
			}

			var response = await base.SendAsync(request, cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				navigationManager.NavigateTo("/account/login");
			}

			return response;
		}
	}
}
