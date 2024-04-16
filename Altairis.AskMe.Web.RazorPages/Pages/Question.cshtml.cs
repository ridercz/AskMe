namespace Altairis.AskMe.Web.RazorPages.Pages;

public class QuestionModel(AskDbContext dc) : PageModel {

    // Model properties

    public Question Data { get; set; } = null!;

    // Handlers

    public async Task<IActionResult> OnGetAsync(int questionId) {
        var data = await dc.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
        if (data == null) return this.NotFound();
        this.Data = data;
        return this.Page();
    }
}
