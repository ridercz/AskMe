using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.Pages {
    public class IndexModel : PagedPageModel<Question> {
        private readonly AskDbContext _dc;
        private readonly AppConfiguration _cfg;
        private readonly IQueryable<Question> _dataSource;

        // Constructor

        public IndexModel(AskDbContext dc, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            _dc = dc;
            _cfg = optionsSnapshot.Value;
            _dataSource = _dc.Questions
                .Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered);
        }

        // Handlers

        public async Task OnGetAsync(int pageNumber) {
            await base.GetData(_dataSource, pageNumber, _cfg.PageSize);
        }

    }
}