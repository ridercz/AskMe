using System.Globalization;
using Microsoft.AspNetCore.Blazor.Hosting;

namespace Havit.AskMe.Web.Blazor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-cz");

			CreateHostBuilder(args).Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
