using Altairis.AskMe.Web.Mvc.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.Mvc.Controllers;

[Route("Account")]
public class AccountController(SignInManager<ApplicationUser> signInManager) : Controller {

    // Actions

    [Route("Login")]
    public IActionResult Login() => this.View();

    [HttpPost, Route("Login")]
    public async Task<IActionResult> Login(LoginModel model, string? returnUrl) {
        if (this.ModelState.IsValid) {
            var result = await signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded) {
                return this.LocalRedirect(returnUrl ?? "/");
            } else {
                this.ModelState.AddModelError(string.Empty, "Přihlášení se nezdařilo");
            }
        }
        return this.View();
    }

    [Route("Logout")]
    public async Task<IActionResult> Logout() {
        await signInManager.SignOutAsync();
        return this.MessageView("Odhlášení", "Byli jste úspěšně odhlášeni");
    }

}
