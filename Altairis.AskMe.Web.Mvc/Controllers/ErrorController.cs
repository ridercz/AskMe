namespace Altairis.AskMe.Web.Mvc.Controllers;

[Route("Error")]
public class ErrorController : Controller {
    [Route("{errorCode:int:min(100):max(599)}")]
    public IActionResult Index(int errorCode) => this.View(errorCode);

    [Route("404")]
    public IActionResult Error404() => this.View("404");

    [Route("500")]
    public IActionResult Error500() => this.View("500");

}
