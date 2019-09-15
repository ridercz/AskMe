using System.Globalization;
using Havit.AskMe.Web.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.AskMe.Web.Blazor.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddTransient<ICategoryClientFacade, CategoryClientFacade>();
            services.AddTransient<IQuestionClientFacade, QuestionClientFacade>();

			services.AddTransient<IJsHelpers, JsHelpers>();
		}

		public void Configure(IComponentsApplicationBuilder app)
        {
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("cs-cz");

			app.AddComponent<App>("app");
        }
    }
}
