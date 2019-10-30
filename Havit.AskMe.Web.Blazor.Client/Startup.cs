using System;
using System.Globalization;
using System.Net.Http;
using Blazor.Extensions.Storage;
using Havit.AskMe.Web.Blazor.Client.Infrastructure;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Microsoft.AspNetCore.Blazor.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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
					new ApiHttpMessageHandler(
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
			services.AddTransient<IAccountClientFacade, AccountClientFacade>();

			// helpers
			services.AddTransient<IJsHelpers, JsHelpers>();
		}

		public void Configure(IComponentsApplicationBuilder app)
		{
			app.UseLocalTimeZone(); // Blazor (mono) fallbacks to UTC. Local time support not implementated yet.

			app.AddComponent<App>("app");
		}
	}
}
