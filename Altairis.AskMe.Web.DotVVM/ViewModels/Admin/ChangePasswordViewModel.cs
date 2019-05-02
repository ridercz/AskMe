using System.ComponentModel.DataAnnotations;
using DotVVM.Framework.Runtime.Filters;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Altairis.AskMe.Data;
using DotVVM.Framework.ViewModel.Validation;

namespace Altairis.AskMe.Web.DotVVM.ViewModels.Admin {
    [Authorize]
    public class ChangePasswordViewModel : Altairis.AskMe.Web.DotVVM.ViewModels.MasterPageViewModel {
        private readonly UserManager<ApplicationUser> userManager;

        public ChangePasswordViewModel(IHostingEnvironment env, UserManager<ApplicationUser> userManager )
            : base(env) {
            this.userManager = userManager;
        }

        public InputModel Input { get; set; } = new InputModel();

        public class InputModel {
            [Required, DataType(DataType.Password)]
            public string OldPassword { get; set; }

            [Required, DataType(DataType.Password), MinLength(12)]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Compare("NewPassword")]
            public string ConfirmPassword { get; set; }
        }

        public string Message { get; set; }

        public override string PageTitle => "Zmìna hesla";

        public async Task ChangePassword() {
            var user = await this.userManager.GetUserAsync(this.Context.HttpContext.User);
            var result = await this.userManager.ChangePasswordAsync(user, this.Input.OldPassword, this.Input.NewPassword);
            if (result.Succeeded) {
                this.Message = "Vaše heslo bylo úspìšnì zmìnìno";
            } else {
                //this.Context.ModelState.Errors.Add(new ViewModelValidationError { ErrorMessage = "Pøihlášení se nezdaøilo" });
                this.AddModelError(x => x.Input, "Zmìna hesla se nezdaøila");
                this.Context.FailOnInvalidModelState();
            }

        }
    }
}

