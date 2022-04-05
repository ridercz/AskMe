using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Admin;

public class ChangePasswordModel : PageModel {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public ChangePasswordModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {
        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MinLength(12)]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync() {
        if (this.ModelState.IsValid) {
            // Get current user
            var user = await this.userManager.GetUserAsync(this.User);

            // Try to change password
            var result = await this.userManager.ChangePasswordAsync(
                user,
                this.Input.OldPassword,
                this.Input.NewPassword);

            if (result.Succeeded) {
                // OK, re-sign and redirect to homepage
                await this.signInManager.SignInAsync(user, isPersistent: false);
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

