using System;
using System.Globalization;
using System.Net.Http;
using Blazor.Extensions.Storage;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.AskMe.Web.Blazor.Client
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddStorage(); // Blazor.Extensions.Storage

			// auth
			services.AddAuthorizationCore();
			services.AddSingleton<ApiAuthenticationStateProvider, ApiAuthenticationStateProvider>();
			services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarder
			services.AddSingleton<IApiAuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarded

			services.AddSingleton(serviceProvider =>
			{
				var navigationManager = serviceProvider.GetService<NavigationManager>();
				return new HttpClient(
					new AuthenticationDelegatingHandler(
						new WebAssemblyHttpMessageHandler(),
						navigationManager,
						serviceProvider.GetRequiredService<ApiAuthenticationStateProvider>()
					))
				{
					BaseAddress = new Uri(navigationManager.BaseUri)
				};
			});

			// facades
			services.AddTransient<ICategoryClientFacade, CategoryClientFacade>();
			services.AddTransient<IQuestionClientFacade, QuestionClientFacade>();
			services.AddTransient<IAuthenticationClientFacade, AuthenticationClientFacade>();

			// helpers
			services.AddTransient<IJsHelpers, JsHelpers>();
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-cz");

			app.AddComponent<App>("app");
		}
	}
}
