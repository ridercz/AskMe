using Altairis.AskMe.Web.Mvc.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.Mvc.Controllers;

[Route("Admin"), Authorize]
public class AdminController(AskDbContext dc, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller {

    // Actions

    [Route("/Question/{questionId:int:min(1)}/edit")]
    public async Task<IActionResult> EditQuestion(int questionId) {
        // Get question
        var q = await dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        // Prepare model
        var model = new EditQuestionModel {
            AnswerText = q.AnswerText,
            CategoryId = q.CategoryId,
            DisplayName = q.DisplayName,
            EmailAddress = q.EmailAddress,
            QuestionText = q.QuestionText,
            Categories = await dc.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToListAsync()
        };

        return this.View(model);
    }

    [HttpPost, Route("/Question/{questionId:int:min(1)}/edit")]
    public async Task<IActionResult> EditQuestion(int questionId, EditQuestionModel model) {
        // Get question
        var q = await dc.Questions.FindAsync(questionId);
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

            await dc.SaveChangesAsync();
            return this.RedirectToAction(
                actionName: "Question",
                controllerName: "Home",
                routeValues: new { questionId = q.Id });
        }
        return this.View(model);
    }

    [Route("/Question/{questionId:int:min(1)}/delete")]
    public async Task<IActionResult> DeleteQuestion(int questionId) {
        // Get question
        var q = await dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        return this.View(new DeleteQuestionModel { QuestionText = q.QuestionText });
    }

    [HttpPost, Route("/Question/{questionId:int:min(1)}/delete")]
    public async Task<IActionResult> DeleteQuestion(int questionId, DeleteQuestionModel model) {
        // Get question
        var q = await dc.Questions.FindAsync(questionId);

        // Delete question
        if (q != null) {
            dc.Remove(q);
            await dc.SaveChangesAsync();
        }
        return this.RedirectToAction("Questions", "Home");
    }

    [Route("ChangePassword")]
    public IActionResult ChangePassword() => this.View();

    [HttpPost, Route("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model) {
        if (this.ModelState.IsValid) {
            // Get current user
            var user = await userManager.GetUserAsync(this.User);
            if (user == null) throw new InvalidOperationException();

            // Try to change password
            var result = await userManager.ChangePasswordAsync(
                user,
                model.OldPassword,
                model.NewPassword);

            if (result.Succeeded) {
                // OK, re-sign and redirect to homepage
                await signInManager.SignInAsync(user, isPersistent: false);
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
