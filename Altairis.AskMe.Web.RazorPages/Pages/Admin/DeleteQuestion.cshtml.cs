namespace Altairis.AskMe.Web.RazorPages.Pages.Admin;

public class DeleteQuestionModel(AskDbContext dc) : PageModel {
    public string QuestionText { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int questionId) {
        var question = await dc.Questions.FindAsync(questionId);
        if (question == null) return this.NotFound();
        this.QuestionText = question.QuestionText;
        return this.Page();
    }

    public async Task<IActionResult> OnPostAsync(int questionId) {
        var question = await dc.Questions.FindAsync(questionId);
        if (question != null) {
            dc.Remove(question);
            await dc.SaveChangesAsync();
        }
        return this.RedirectToPage("/Questions");
    }

}
