using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account {
    public class LoginModel : PageModel {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager) {
            this._signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required]
            public string UserName { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = "/") {
            if (this.ModelState.IsValid) {
                var result = await this._signInManager.PasswordSignInAsync(
                    this.Input.UserName,
                    this.Input.Password,
                    this.Input.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded) {
                    return this.LocalRedirect(returnUrl);
                } else {
                    this.ModelState.AddModelError(string.Empty, "Přihlášení se nezdařilo");
                }
            }
            return this.Page();
        }
    }
}