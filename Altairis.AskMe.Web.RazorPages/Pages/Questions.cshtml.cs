using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.RazorPages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.RazorPages.Pages {
    public class QuestionsModel : PagedPageModel<Question> {
        private readonly AskDbContext dbContext;
        private readonly AppConfiguration config;
        private readonly IQueryable<Question> dataSource;

        // Constructor

        public QuestionsModel(AskDbContext dbContext, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
            this.dbContext = dbContext;
            this.config = optionsSnapshot.Value;
            this.dataSource = this.dbContext.Questions
                .Include(x => x.Category)
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated);
        }

        // Model properties

        public IEnumerable<SelectListItem> Categories => this.dbContext.Categories
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
            await base.GetData(this.dataSource, pageNumber, this.config.PageSize);
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
                await this.dbContext.Questions.AddAsync(nq);
                await this.dbContext.SaveChangesAsync();

                // Redirect to list of questions
                return this.RedirectToPage(pageName: "Questions", pageHandler: null, routeValues: new { pageNumber = string.Empty }, fragment: $"q_{nq.Id}");
            }

            await base.GetData(this.dataSource, pageNumber, this.config.PageSize);
            return this.Page();
        }

    }
}