using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account;

public class LogoutModel : PageModel {
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(SignInManager<ApplicationUser> signInManager) {
        this._signInManager = signInManager;
    }

    public async Task OnGetAsync() => await this._signInManager.SignOutAsync();
}
