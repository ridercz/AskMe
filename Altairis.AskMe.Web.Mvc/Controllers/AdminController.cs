using Altairis.AskMe.Web.Mvc.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.Mvc.Controllers;

[Route("Admin"), Authorize]
public class AdminController : Controller {
    private readonly AskDbContext dc;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    // Constructor

    public AdminController(AskDbContext dc, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
        this.dc = dc;
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    // Actions

    [Route("/question/{questionId:int:min(1)}/edit")]
    public async Task<IActionResult> Index(int questionId) {
        // Get question
        var q = await this.dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        // Prepare model
        var model = new IndexModel {
            AnswerText = q.AnswerText,
            CategoryId = q.CategoryId,
            DisplayName = q.DisplayName,
            EmailAddress = q.EmailAddress,
            QuestionText = q.QuestionText,
            Categories = await this.dc.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToListAsync()
        };

        return this.View(model);
    }

    [HttpPost, Route("{questionId:int:min(1)}")]
    public async Task<IActionResult> Index(int questionId, IndexModel model) {
        // Get question
        var q = await this.dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        if (this.ModelState.IsValid) {
            // Update question
            q.CategoryId = model.CategoryId;
            q.DisplayName = model.DisplayName;
            q.EmailAddress = model.EmailAddress;
            q.QuestionText = model.QuestionText;

            if (string.IsNullOrWhiteSpace(model.AnswerText)) {
                q.AnswerText = null;
                q.DateAnswered = null;
            } else {
                q.AnswerText = model.AnswerText;
                if (!q.DateAnswered.HasValue) q.DateAnswered = DateTime.Now;
            }

            await this.dc.SaveChangesAsync();
            return this.RedirectToAction(
                actionName: "Question",
                controllerName: "Home",
                routeValues: new { questionId = q.Id });
        }
        return this.View(model);
    }

    [Route("ChangePassword")]
    public IActionResult ChangePassword() => this.View();

    [HttpPost, Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model) {
        if (this.ModelState.IsValid) {
            // Get current user
            var user = await this.userManager.GetUserAsync(this.User);

            // Try to change password
            var result = await this.userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword);

            if (result.Succeeded) {
                // OK, re-sign and redirect to homepage
                await this.signInManager.SignInAsync(user, isPersistent: false);
                return this.MessageView("Změna hesla", "Vaše heslo bylo úspěšně změněno.");
            } else {
                // Failed - show why
                foreach (var error in result.Errors) {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return this.View(model);
    }
}
