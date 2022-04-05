using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account;

public class LogoutModel : PageModel {
    private readonly SignInManager<ApplicationUser> signInManager;

    public LogoutModel(SignInManager<ApplicationUser> signInManager) {
        this.signInManager = signInManager;
    }

    public async Task OnGetAsync() => await this.signInManager.SignOutAsync();
}
