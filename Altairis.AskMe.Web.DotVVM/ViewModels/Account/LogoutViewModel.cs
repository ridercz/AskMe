using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.DotVVM.ViewModels.Account {
    public class LogoutViewModel : Altairis.AskMe.Web.DotVVM.ViewModels.MasterPageViewModel {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutViewModel(SignInManager<ApplicationUser> signInManager, IHostingEnvironment env)
        : base(env) {
            this._signInManager = signInManager;
        }

        public override string PageTitle => "Odhlášení";

        public override async Task Load() => await this._signInManager.SignOutAsync();

    }
}

