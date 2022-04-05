using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.RazorPages.Pages; 
public class IndexModel : PagedPageModel<Question> {
    private readonly AskDbContext _dc;
    private readonly AppSettings _cfg;
    private readonly IQueryable<Question> _dataSource;

    // Constructor

    public IndexModel(AskDbContext dc, IOptionsSnapshot<AppSettings> optionsSnapshot) {
        this._dc = dc;
        this._cfg = optionsSnapshot.Value;
        this._dataSource = this._dc.Questions
            .Include(x => x.Category)
            .Where(x => x.DateAnswered.HasValue)
            .OrderByDescending(x => x.DateAnswered);
    }

    // Handlers

    public async Task OnGetAsync(int pageNumber) => await base.GetData(this._dataSource, pageNumber, this._cfg.PageSize);

}
