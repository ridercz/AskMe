using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.AskMe.Web.DotVVM {
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator {
        public void Configure(DotvvmConfiguration config, string applicationPath) {
            config.RouteTable.Add("Questions", "questions/{pageNumber?}", "Views/Questions.dothtml", defaultValues: new { pageNumber = 1 });
            config.RouteTable.Add("Home", "{pageNumber?}", "Views/Index.dothtml", defaultValues: new { pageNumber = 1 });

            config.Resources.Register("Styles", new StylesheetResource(new FileResourceLocation("wwwroot/Styles/askme.css")));
            config.Markup.AddMarkupControl("my", "pager", "Controls/Pager.dotcontrol");
        }

        public void ConfigureServices(IDotvvmServiceCollection options) {
            // TODO: Configure services
        }
    }
}
