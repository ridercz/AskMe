using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Mvc {
    public static class ControllerExtensions {

        public static IActionResult MessageView(this Controller controller, string title, string message) => controller.View("Message", (title, message));

    }
}
