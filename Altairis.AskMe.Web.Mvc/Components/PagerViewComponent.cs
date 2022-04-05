using Altairis.AskMe.Web.Mvc.Models;

namespace Altairis.AskMe.Web.Mvc.Components;
public class PagerViewComponent : ViewComponent {

    public IViewComponentResult Invoke(PagingInfo model) => this.View(model);

}
