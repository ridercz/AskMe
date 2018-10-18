using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Components {
    public class MenuViewComponent : ViewComponent {

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync() {
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            if (!this.User.Identity.IsAuthenticated) {
                return this.View("Anonymous");
            }
            else {
                return this.View("Authenticated");
            }
        }

    }
}
