using Microsoft.AspNetCore.Identity;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account;

public class LogoutModel(SignInManager<ApplicationUser> signInManager) : PageModel {
    public async Task OnGetAsync() => await signInManager.SignOutAsync();
}
