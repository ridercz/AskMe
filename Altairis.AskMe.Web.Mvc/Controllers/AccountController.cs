using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Mvc.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Mvc.Controllers {
    [Route("account")]
    public class AccountController : Controller {

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager) {
            this._signInManager = signInManager;
        }

        [Route("login")]
        public IActionResult Login() => this.View();

        [HttpPost, Route("login")]
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

        [Route("logout")]
        public async Task<IActionResult> Logout() {
            await this._signInManager.SignOutAsync();
            return this.MessageView("Odhlášení", "Byli jste úspěšně odhlášeni");
        }

    }
}
