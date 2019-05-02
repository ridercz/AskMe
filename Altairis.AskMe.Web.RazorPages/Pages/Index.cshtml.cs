using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.RazorPages.Pages {
    public class IndexModel : PagedPageModel<Question> {
        private readonly AskDbContext dbContext;
        private readonly AppConfiguration config;
        private readonly IQueryable<Question> dataSource;

        // Constructor

        public IndexModel(AskDbContext dbContext, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this.dbContext = dbContext;
            this.config = optionsSnapshot.Value;
            this.dataSource = this.dbContext.Questions
                .Include(x => x.Category)
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered);
        }

        // Handlers

        public async Task OnGetAsync(int pageNumber) {
            await base.GetData(this.dataSource, pageNumber, this.config.PageSize);
        }

    }
}