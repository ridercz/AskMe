using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.RazorPages.Pages;

public class QuestionsModel : PagedPageModel<Question> {
    private readonly AskDbContext dc;
    private readonly AppSettings cfg;
    private readonly IQueryable<Question> dataSource;

    // Constructor

    public QuestionsModel(AskDbContext dc, IOptionsSnapshot<AppSettings> optionsSnapshot) {
        this.dc = dc;
        this.cfg = optionsSnapshot.Value;
        this.dataSource = this.dc.Questions
            .Include(x => x.Category)
            .Where(x => !x.DateAnswered.HasValue)
            .OrderByDescending(x => x.DateCreated);
    }

    // Model properties

    public IEnumerable<SelectListItem> Categories => this.dc.Categories
        .OrderBy(c => c.Name)
        .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    // Input model

    public class InputModel {
        [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
        public string QuestionText { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
        public string? EmailAddress { get; set; }

        public int CategoryId { get; set; }
    }

    // Handlers

    public async Task OnGetAsync(int pageNumber) => await base.GetData(this.dataSource, pageNumber, this.cfg.PageSize);

    public async Task<IActionResult> OnPostAsync(int pageNumber) {
        if (this.ModelState.IsValid) {
            // Create and save question entity
            var nq = new Question {
                QuestionText = this.Input.QuestionText,
                CategoryId = this.Input.CategoryId,
                DisplayName = this.Input.DisplayName,
                EmailAddress = this.Input.EmailAddress
            };
            await this.dc.Questions.AddAsync(nq);
            await this.dc.SaveChangesAsync();

            // Redirect to list of questions
            return this.RedirectToPage(pageName: "Questions", pageHandler: null, routeValues: new { pageNumber = string.Empty }, fragment: $"q_{nq.Id}");
        }

        await base.GetData(this.dataSource, pageNumber, this.cfg.PageSize);
        return this.Page();
    }

}
