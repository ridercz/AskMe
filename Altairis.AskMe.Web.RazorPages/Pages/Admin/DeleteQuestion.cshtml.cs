namespace Altairis.AskMe.Web.RazorPages.Pages.Admin;

public class DeleteQuestionModel : PageModel {
    private readonly AskDbContext dc;

    public DeleteQuestionModel(AskDbContext dc) {
        this.dc = dc;
    }

    public string QuestionText { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int questionId) {
        var question = await this.dc.Questions.FindAsync(questionId);
        if (question == null) return this.NotFound();
        this.QuestionText = question.QuestionText;
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int questionId) {
        var question = await this.dc.Questions.FindAsync(questionId);
        if (question != null) {
            this.dc.Remove(question);
            await this.dc.SaveChangesAsync();
        }
        return this.RedirectToPage("/Questions");
    }

}
