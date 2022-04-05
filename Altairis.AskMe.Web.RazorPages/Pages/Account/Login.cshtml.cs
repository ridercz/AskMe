using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account;

public class LoginModel : PageModel {
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager) {
        this._signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

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
