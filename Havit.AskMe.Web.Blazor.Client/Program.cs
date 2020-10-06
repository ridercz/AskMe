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
			builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddStorage(); // Blazor.Extensions.Storage

			// auth
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<ApiAuthenticationStateProvider, ApiAuthenticationStateProvider>();
			builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarder
			builder.Services.AddScoped<IApiAuthenticationStateProvider>(sp => sp.GetRequiredService<ApiAuthenticationStateProvider>()); // forwarded

			builder.Services.AddScoped(serviceProvider => {
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

			await host.RunAsync();
		}
	}
}
