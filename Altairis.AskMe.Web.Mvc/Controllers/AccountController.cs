using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Mvc.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Mvc.Controllers {
    [Route("Account")]
    public class AccountController : Controller {
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Controller

        public AccountController(SignInManager<ApplicationUser> signInManager) {
            this._signInManager = signInManager;
        }

        // Actions

        [Route("Login")]
        public IActionResult Login() => this.View();

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl = "/") {
            if (this.ModelState.IsValid) {
                var result = await this._signInManager.PasswordSignInAsync(
                    model.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded) {
                    return this.LocalRedirect(returnUrl);
                } else {
                    this.ModelState.AddModelError(string.Empty, "Přihlášení se nezdařilo");
                }
            }
            return this.View();
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout() {
            await this._signInManager.SignOutAsync();
            return this.MessageView("Odhlášení", "Byli jste úspěšně odhlášeni");
        }

    }
}
