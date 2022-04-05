namespace Altairis.AskMe.Web.RazorPages.Components;

public class MenuViewComponent : ViewComponent {
    public IViewComponentResult Invoke() {
        if (this.User.Identity?.IsAuthenticated ?? false) {
            return this.View("Authenticated");
        } else {
            return this.View("Anonymous");
        }
    }

}
