
namespace Altairis.AskMe.Web.RazorPages.Pages;
public class QuestionModel : PageModel {
    private readonly AskDbContext _dc;


    // Constructor

    public QuestionModel(AskDbContext dc) {
        this._dc = dc;
    }

    // Model properties

    public Question Data { get; set; }

    // Handlers

    public async Task<IActionResult> OnGetAsync(int questionId) {
        this.Data = await this._dc.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
        if (this.Data == null) return this.NotFound();
        return this.Page();
    }
}
