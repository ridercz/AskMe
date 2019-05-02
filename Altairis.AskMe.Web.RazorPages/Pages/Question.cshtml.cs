using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Web.RazorPages.Pages {
    public class QuestionModel : PageModel {
        private readonly AskDbContext dbContext;

        // Constructor

        public QuestionModel(AskDbContext dbContext) {
            this.dbContext = dbContext;
        }

        // Model properties

        public Question Data { get; set; }

        // Handlers

        public async Task<IActionResult> OnGetAsync(int questionId) {
            this.Data = await this.dbContext.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
            return this.Data == null ? this.NotFound() : (IActionResult)this.Page();
        }
    }
}