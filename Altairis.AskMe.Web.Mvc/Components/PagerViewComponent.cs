using System.Threading.Tasks;
using Altairis.AskMe.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Mvc.Components {
    public class PagerViewComponent : ViewComponent {

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IViewComponentResult> InvokeAsync(PagingInfo model) {
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            return this.View(model);
        }

    }
}
