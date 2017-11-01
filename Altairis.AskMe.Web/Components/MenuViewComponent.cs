using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Identity;
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
