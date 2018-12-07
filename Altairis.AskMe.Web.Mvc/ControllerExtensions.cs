using Microsoft.AspNetCore.Mvc;

namespace Altairis.AskMe.Web.Mvc {
    public static class ControllerExtensions {

        public static IActionResult MessageView(this Controller controller, string title, string message) => controller.View("Message", (title, message));

    }
}
