using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public abstract class MasterPageViewModel : DotvvmViewModelBase {
        public string EnvironmentName { get; }

        public abstract string PageTitle { get; }

        public MasterPageViewModel(IHostingEnvironment env) {
            this.EnvironmentName = env.EnvironmentName;
        }

    }
}

