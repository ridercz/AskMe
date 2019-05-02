using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.DotVVM.ViewModels.Account {
    public class LogoutViewModel : Altairis.AskMe.Web.DotVVM.ViewModels.MasterPageViewModel {
        private readonly SignInManager<ApplicationUser> signInManager;

        public LogoutViewModel(SignInManager<ApplicationUser> signInManager, IHostingEnvironment env) : base(env) {
            this.signInManager = signInManager;
        }

        public override string PageTitle => "Odhlášení";

        public override async Task Load() => await this.signInManager.SignOutAsync();

    }
}

