using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Web.Pages {
    public class QuestionModel : PageModel {
        private readonly AskDbContext _dc;


        // Constructor

        public QuestionModel(AskDbContext dc) {
            _dc = dc;
        }

        // Model properties

        public Question Data { get; set; }

        // Handlers

        public async Task<IActionResult> OnGetAsync(int questionId) {
            this.Data = await _dc.Questions.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == questionId);
            if (this.Data == null) return this.NotFound();
            return this.Page();
        }
    }
}