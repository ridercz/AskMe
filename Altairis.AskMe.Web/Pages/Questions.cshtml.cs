using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.AskMe.Web.Pages {
    public class QuestionsModel : PageModel {
        private readonly AskDbContext _dc;

        public QuestionsModel(AskDbContext dc) {
            _dc = dc;
        }

        public IEnumerable<SelectListItem> Categories => _dc.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
            public string QuestionText { get; set; }

            [MaxLength(100)]
            public string DisplayName { get; set; }

            [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
            public string EmailAddress { get; set; }

            public int CategoryId { get; set; }
        }

        public async Task<IActionResult> OnPostAsync() {
            // Validate input
            if (!this.ModelState.IsValid) return this.Page();

            // Create and save question entity
            var nq = new Question {
                QuestionText = this.Input.QuestionText,
                CategoryId = this.Input.CategoryId,
                DisplayName = this.Input.DisplayName,
                EmailAddress = this.Input.EmailAddress
            };
            await _dc.Questions.AddAsync(nq);
            await _dc.SaveChangesAsync();

            // Redirect to list of questions
            return this.RedirectToPage(pageName: "Questions", pageHandler: null, fragment: $"q_{nq.Id}");
        }

    }
}