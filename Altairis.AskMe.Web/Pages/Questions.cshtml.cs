using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.Pages {
    public class QuestionsModel : PagedPageModel<Question> {
        private readonly AskDbContext _dc;
        private readonly AppConfiguration _cfg;
        private readonly IQueryable<Question> _dataSource;

        // Constructor

        public QuestionsModel(AskDbContext dc, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this._dc = dc;
            this._cfg = optionsSnapshot.Value;
            this._dataSource = this._dc.Questions
                .Include(x => x.Category)
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated);
        }

        // Model properties

        public IEnumerable<SelectListItem> Categories => this._dc.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        [BindProperty]
        public InputModel Input { get; set; }

        // Input model

        public class InputModel {
            [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
            public string QuestionText { get; set; }

            [MaxLength(100)]
            public string DisplayName { get; set; }

            [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
            public string EmailAddress { get; set; }

            public int CategoryId { get; set; }
        }

        // Handlers

        public async Task OnGetAsync(int pageNumber) {
            await base.GetData(this._dataSource, pageNumber, this._cfg.PageSize);
        }

        public async Task<IActionResult> OnPostAsync(int pageNumber) {
            if (this.ModelState.IsValid) {
                // Create and save question entity
                var nq = new Question {
                    QuestionText = this.Input.QuestionText,
                    CategoryId = this.Input.CategoryId,
                    DisplayName = this.Input.DisplayName,
                    EmailAddress = this.Input.EmailAddress
                };
                await this._dc.Questions.AddAsync(nq);
                await this._dc.SaveChangesAsync();

                // Redirect to list of questions
                return this.RedirectToPage(pageName: "Questions", pageHandler: null, fragment: $"q_{nq.Id}");
            }

            await base.GetData(this._dataSource, pageNumber, this._cfg.PageSize);
            return this.Page();
        }

    }
}