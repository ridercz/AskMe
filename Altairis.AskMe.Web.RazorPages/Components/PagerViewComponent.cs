using System.Threading.Tasks;
using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.RazorPages.Components {
    public class PagerViewComponent : ViewComponent {

        public IViewComponentResult Invoke(PagingInfo model) => this.View(model);

    }
}
