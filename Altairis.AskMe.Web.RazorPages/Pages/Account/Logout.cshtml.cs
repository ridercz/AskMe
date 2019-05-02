using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.AskMe.Web.RazorPages.Pages.Account {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ApplicationUser> signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager) {
            this.signInManager = signInManager;
        }

        public async Task OnGetAsync() {
            await this.signInManager.SignOutAsync();
        }
    }
}