using Altairis.AskMe.Web.Mvc.Models;
using Altairis.AskMe.Web.Mvc.Models.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.Mvc.Controllers;

public class HomeController(AskDbContext dc, IOptionsSnapshot<AppSettings> optionsSnapshot, UserManager<ApplicationUser> userManager) : Controller {
    private readonly AppSettings cfg = optionsSnapshot.Value;

    // Actions

    [Route("{pageNumber:int:min(1)=1}")]
    public async Task<IActionResult> Index(int pageNumber) {
        if (!await dc.Users.AnyAsync()) return this.RedirectToAction("FirstRun");
        var model = new PagedModel<Question>();
        var query = dc.Questions
            .Include(x => x.Category)
            .Where(x => x.DateAnswered.HasValue)
            .OrderByDescending(x => x.DateAnswered);
        await model.GetData(query, pageNumber, this.cfg.PageSize);
        return this.View(model);
    }

    [Route("FirstRun")]
    public async Task<IActionResult> FirstRun() => await dc.Users.AnyAsync() ? this.NotFound() : this.View();

    [Route("FirstRun"), HttpPost]
    public async Task<IActionResult> FirstRun(FirstRunModel model) {
        if (await dc.Users.AnyAsync()) return this.NotFound();

        // Create new user
        if (this.ModelState.IsValid) {
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = model.UserName }, password: model.Password);
            if (result.Succeeded) {
                if (model.SeedDemoData) dc.Seed();
                return this.RedirectToAction("Index");
            }
            foreach (var item in result.Errors) {
                this.ModelState.AddModelError(string.Empty, $"{item.Description} [{item.Code}]");
            }
        }
        return this.View(model);
    }


    [Route("Question/{questionId:int:min(1)}")]
    public async Task<IActionResult> Question(int questionId) {
        var model = await dc.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
        return model == null ? this.NotFound() : this.View(model);
    }

    [Route("Questions/{pageNumber:int:min(1)=1}")]
    public async Task<IActionResult> Questions(int pageNumber) {
        var model = new QuestionsModel {
            Categories = await dc.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToListAsync()
        };

        var query = dc.Questions
            .Include(x => x.Category)
            .Where(x => !x.DateAnswered.HasValue)
            .OrderByDescending(x => x.DateCreated);
        await model.GetData(query, pageNumber, this.cfg.PageSize);

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
            await dc.Questions.AddAsync(nq);
            await dc.SaveChangesAsync();

            // Redirect to list of questions
            return this.RedirectToAction(
                actionName: "Questions",
                controllerName: "Home",
                routeValues: new { pageNumber = string.Empty },
                fragment: $"q_{nq.Id}");
        } else {
            // Invalid data
            model.Categories = await dc.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
                .ToListAsync();
            var query = dc.Questions
                .Include(x => x.Category)
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated);
            await model.GetData(query, pageNumber, this.cfg.PageSize);
            return this.View(model);
        }
    }

    [Route("Contact")]
    public IActionResult Contact() => this.View();

}
