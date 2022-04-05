using Altairis.AskMe.Web.RazorPages.Models;

namespace Altairis.AskMe.Web.RazorPages.Components;

public class PagerViewComponent : ViewComponent {

    public IViewComponentResult Invoke(PagingInfo model) => this.View(model);

}
