using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.RazorPages.Pages;

public class IndexModel : PagedPageModel<Question> {
    private readonly AskDbContext dc;
    private readonly AppSettings cfg;
    private readonly IQueryable<Question> dataSource;

    // Constructor

    public IndexModel(AskDbContext dc, IOptionsSnapshot<AppSettings> optionsSnapshot) {
        this.dc = dc;
        this.cfg = optionsSnapshot.Value;
        this.dataSource = this.dc.Questions
            .Include(x => x.Category)
            .Where(x => x.DateAnswered.HasValue)
            .OrderByDescending(x => x.DateAnswered);
    }

    // Handlers

    public async Task OnGetAsync(int pageNumber) => await base.GetData(this.dataSource, pageNumber, this.cfg.PageSize);

}
