using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Mvc.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Web.Mvc.Controllers {

    [Route("Admin"), Authorize]
    public class AdminController : Controller {
        private readonly AskDbContext _dc;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // Constructor

        public AdminController(AskDbContext dc, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            this._dc = dc;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        // Actions

        [Route("{questionId:int:min(1)}")]
        public async Task<IActionResult> Index(int questionId) {
            // Get question
            var q = await this._dc.Questions.FindAsync(questionId);
            if (q == null) return this.NotFound();

            // Prepare model
            var model = new IndexModel {
                AnswerText = q.AnswerText,
                CategoryId = q.CategoryId,
                DisplayName = q.DisplayName,
                EmailAddress = q.EmailAddress,
                QuestionText = q.QuestionText,
                Categories = await this._dc.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync()
            };

            return this.View(model);
        }

        [HttpPost, Route("{questionId:int:min(1)}")]
        public async Task<IActionResult> Index(int questionId, IndexModel model) {
            // Get question
            var q = await this._dc.Questions.FindAsync(questionId);
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

                await this._dc.SaveChangesAsync();
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
        public async Task< IActionResult> ChangePassword(ChangePasswordModel model) {
            if (this.ModelState.IsValid) {
                // Get current user
                var user = await this._userManager.GetUserAsync(this.User);

                // Try to change password
                var result = await this._userManager.ChangePasswordAsync(
                    user,
                    model.OldPassword,
                    model.NewPassword);

                if (result.Succeeded) {
                    // OK, re-sign and redirect to homepage
                    await this._signInManager.SignInAsync(user, isPersistent: false);
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
}
