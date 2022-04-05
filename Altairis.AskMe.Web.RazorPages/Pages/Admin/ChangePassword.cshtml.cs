using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Admin; 
public class ChangePasswordModel : PageModel {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
        this._userManager = userManager;
        this._signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel {
        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password), MinLength(12)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPostAsync() {
        if (this.ModelState.IsValid) {
            // Get current user
            var user = await this._userManager.GetUserAsync(this.User);

            // Try to change password
            var result = await this._userManager.ChangePasswordAsync(
                user,
                this.Input.OldPassword,
                this.Input.NewPassword);

            if (result.Succeeded) {
                // OK, re-sign and redirect to homepage
                await this._signInManager.SignInAsync(user, isPersistent: false);
                return this.RedirectToPage("ChangePasswordDone");
            } else {
                // Failed - show why
                foreach (var error in result.Errors) {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

        }
        return this.Page();
    }
}

