using System.Linq;
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
        private readonly AskDbContext dbContext;
        private readonly AppConfiguration config;

        // Constructor

        public HomeController(AskDbContext dbContext, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this.dbContext = dbContext;
            this.config = optionsSnapshot.Value;
        }

        // Actions

        [Route("{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Index(int pageNumber) {
            var model = new PagedModel<Question>();
            var query = this.dbContext.Questions
                .Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered);
            await model.GetData(query, pageNumber, this.config.PageSize);
            return this.View(model);
        }

        [Route("Question/{questionId:int:min(1)}")]
        public async Task<IActionResult> Question(int questionId) {
            var model = await this.dbContext.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
            if (model == null) return this.NotFound();
            return this.View(model);
        }

        [Route("Questions/{pageNumber:int:min(1)=1}")]
        public async Task<IActionResult> Questions(int pageNumber) {
            var model = new QuestionsModel {
                Categories = await this.dbContext.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync()
            };

            var query = this.dbContext.Questions
                .Include(x => x.Category)
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated);
            await model.GetData(query, pageNumber, this.config.PageSize);

            return this.View(model);
        }

        [HttpPost, Route("Questions/{pageNumber:int:min(1)=1}")]
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
                await this.dbContext.Questions.AddAsync(nq);
                await this.dbContext.SaveChangesAsync();

                // Redirect to list of questions
                return this.RedirectToAction(
                    actionName: "Questions",
                    controllerName: "Home",
                    routeValues: new { pageNumber = string.Empty },
                    fragment: $"q_{nq.Id}");
            } else {
                // Invalid data
                model.Categories = await this.dbContext.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                    .ToListAsync();
                var query = this.dbContext.Questions
                    .Include(x => x.Category)
                    .Where(x => !x.DateAnswered.HasValue)
                    .OrderByDescending(x => x.DateCreated);
                await model.GetData(query, pageNumber, this.config.PageSize);
                return this.View(model);
            }
        }

        [Route("Contact")]
        public IActionResult Contact() => this.View();

    }
}
