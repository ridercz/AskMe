using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.RazorPages.Pages.Admin;

public class IndexModel : PageModel {
    private readonly AskDbContext dc;

    // Constructor

    public IndexModel(AskDbContext dc) {
        this.dc = dc;
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

        public string? AnswerText { get; set; }

        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
        public string? EmailAddress { get; set; }

        public int CategoryId { get; set; }
    }

    // Handlers

    public async Task<IActionResult> OnGetAsync(int questionId) {
        // Get question
        var q = await this.dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        // Prepare model
        this.Input = new InputModel {
            AnswerText = q.AnswerText,
            CategoryId = q.CategoryId,
            DisplayName = q.DisplayName,
            EmailAddress = q.EmailAddress,
            QuestionText = q.QuestionText
        };

        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int questionId) {
        // Get question
        var q = await this.dc.Questions.FindAsync(questionId);
        if (q == null) return this.NotFound();

        if (this.ModelState.IsValid) {
            // Update question
            q.CategoryId = this.Input.CategoryId;
            q.DisplayName = this.Input.DisplayName;
            q.EmailAddress = this.Input.EmailAddress;
            q.QuestionText = this.Input.QuestionText;

            if (string.IsNullOrWhiteSpace(this.Input.AnswerText)) {
                q.AnswerText = null;
                q.DateAnswered = null;
            } else {
                q.AnswerText = this.Input.AnswerText;
                if (!q.DateAnswered.HasValue) q.DateAnswered = DateTime.Now;
            }

            await this.dc.SaveChangesAsync();
            return this.RedirectToPage("/Question", new { questionId = q.Id });
        }
        return this.Page();
    }
}
