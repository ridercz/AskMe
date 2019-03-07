using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.ViewModel.Validation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.DotVVM.ViewModels.Account {
    public class LoginViewModel : Altairis.AskMe.Web.DotVVM.ViewModels.MasterPageViewModel {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginViewModel(SignInManager<ApplicationUser> signInManager, IHostingEnvironment env)
        : base(env) {
            this._signInManager = signInManager;
        }

        public InputModel Input { get; set; } = new InputModel();

        public override string PageTitle => "Pøihlášení";

        public class InputModel {
            [Required]
            public string UserName { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task Login() {
            var result = await this._signInManager.PasswordSignInAsync(
                    this.Input.UserName,
                    this.Input.Password,
                    this.Input.RememberMe,
                    lockoutOnFailure: false);

            if (result.Succeeded) {
                var url = this.Context.Query.TryGetValue("returnUrl", out var x) ? x : "/";
                this.Context.RedirectToUrl(url);
            }
            else {
                //this.Context.ModelState.Errors.Add(new ViewModelValidationError { ErrorMessage = "Pøihlášení se nezdaøilo" });
                this.AddModelError(x => x.Input, "Pøihlášení se nezdaøilo");
                this.Context.FailOnInvalidModelState();
            }
        }
    }
}