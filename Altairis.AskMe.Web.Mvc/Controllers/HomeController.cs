using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Mvc.Models;
using Altairis.AskMe.Web.Mvc.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.Mvc.Controllers {
    public class HomeController : Controller {
        private readonly AskDbContext _dc;
        private readonly AppConfiguration _cfg;

        // Constructor

        public HomeController(AskDbContext dc, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this._dc = dc;
            this._cfg = optionsSnapshot.Value;

        }

        // Action methods

        [Route("{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Index(int pageNumber) {
            var model = new PagedModel<Question>();
            var query = this._dc.Questions
                .Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered);
            await model.GetData(query, pageNumber, this._cfg.PageSize);
            return this.View(model);
        }

        [Route("question/{questionId:int:min(1)}")]
        public async Task<IActionResult> Question(int questionId) {
            var model = await this._dc.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
            if (model == null) return this.NotFound();
            return this.View(model);
        }

        [Route("questions/{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Questions(int pageNumber) {
            var model = new QuestionsModel {
                Categories = await this._dc.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync()
            };

            var query = this._dc.Questions
                .Include(x => x.Category)
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated);
            await model.GetData(query, pageNumber, this._cfg.PageSize);

            return this.View(model);
        }

        [HttpPost, Route("questions/{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Questions(int pageNumber, QuestionsModel model) {
            // Validate posted data
            if (this.ModelState.IsValid) {
                // Create and save question entity
                var nq = new Question {
                    QuestionText = model.Input.QuestionText,
                    CategoryId = model.Input.CategoryId,
                    DisplayName = model.Input.DisplayName,
                    EmailAddress = model.Input.EmailAddress
                };
                await this._dc.Questions.AddAsync(nq);
                await this._dc.SaveChangesAsync();

                // Redirect to list of questions
                return this.RedirectToAction(
                    actionName: "Questions",
                    controllerName: "Home",
                    routeValues: new { pageNumber = string.Empty },
                    fragment: $"q_{nq.Id}");
            } else {
                // Invalid data
                model.Categories = await this._dc.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync();
                var query = this._dc.Questions
                    .Include(x => x.Category)
                    .Where(x => !x.DateAnswered.HasValue)
                    .OrderByDescending(x => x.DateCreated);
                await model.GetData(query, pageNumber, this._cfg.PageSize);
                return this.View(model);
            }
        }
    }
}
