using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.AskMe.Web.Pages.Account {
    public class LogoutModel : PageModel {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutModel(SignInManager<ApplicationUser> signInManager) {
            _signInManager = signInManager;
        }

        public async Task OnGetAsync() {
            await _signInManager.SignOutAsync();
        }
    }
}