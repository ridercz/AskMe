namespace Altairis.AskMe.Web.Mvc.Components;
public class MenuViewComponent : ViewComponent {
    public IViewComponentResult Invoke() {
        if (!this.User.Identity.IsAuthenticated) {
            return this.View("Anonymous");
        } else {
            return this.View("Authenticated");
        }
    }

}
