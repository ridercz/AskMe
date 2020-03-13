using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Extensions.Storage;
using Havit.AskMe.Web.Blazor.Client.Infrastructure;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Client.Services.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Havit.AskMe.Web.Blazor.Client
{
	public class Program
    {
        public static async Task Main(string[] args)
        {
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-cz");

			var builder = WebAssemblyHostBuilder.CreateDefault();
			builder.RootComponents.Add<App>("app");

			builder.Services.AddOptions();
			builder.Services.AddBaseAddressHttpClient();

			builder.Services.AddStorage(); // Blazor.Extensions.Storage

			// auth
			builder.Services.AddAuthorizationCore();
			builder.Services.AddSingleton<ApiAuthenticationStateProvider, ApiAuthenticationStateProvider>();
			builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarder
			builder.Services.AddSingleton<IApiAuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarded

			builder.Services.AddSingleton(serviceProvider => {
				var navigationManager = serviceProvider.GetService<NavigationManager>();
				return new HttpClient(
					new ApiHttpMessageHandler(
						new HttpClientHandler(),
						navigationManager,
						serviceProvider.GetRequiredService<ApiAuthenticationStateProvider>()
					))
				{
					BaseAddress = new Uri(navigationManager.BaseUri)
				};
			});

			// facades
			builder.Services.AddTransient<ICategoryClientFacade, CategoryClientFacade>();
			builder.Services.AddTransient<IQuestionClientFacade, QuestionClientFacade>();
			builder.Services.AddTransient<IAuthenticationClientFacade, AuthenticationClientFacade>();
			builder.Services.AddTransient<IAccountClientFacade, AccountClientFacade>();

			// helpers
			builder.Services.AddTransient<IJsHelpers, JsHelpers>();

			var host = builder.Build();

			host.UseLocalTimeZone(); // Blazor (mono) fallbacks to UTC. Local time support not implementated yet.

			await host.RunAsync();
		}
	}
}
