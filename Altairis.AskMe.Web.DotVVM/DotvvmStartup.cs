using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.AskMe.Web.DotVVM {
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator {
        public void Configure(DotvvmConfiguration config, string applicationPath) {
            config.RouteTable.Add("Home", "", "Views/Index.dothtml");
        }

        public void ConfigureServices(IDotvvmServiceCollection options) {
            // TODO: Configure services
        }
    }
}
