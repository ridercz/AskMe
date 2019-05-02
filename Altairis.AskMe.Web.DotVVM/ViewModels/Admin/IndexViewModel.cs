using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.DotVVM.Dto;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;

namespace Altairis.AskMe.Web.DotVVM.ViewModels.Admin {
    public class IndexViewModel : Altairis.AskMe.Web.DotVVM.ViewModels.MasterPageViewModel {
        private readonly AskDbContext dbContext;

        public IndexViewModel(IHostingEnvironment env, AskDbContext dbContext) : base(env) {
            this.dbContext = dbContext;
        }

        public override string PageTitle => "Editace otázky";

        [Bind(Direction.ServerToClientFirstRequest)]
        public IEnumerable<SelectListItemDto> Categories => this.dbContext.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItemDto { Text = c.Name, Value = c.Id });

        public InputModel Input { get; set; }

        private async Task InitializeInputModel() {
            var questionId = Convert.ToInt32(this.Context.Parameters["questionId"]);
            var q = await this.dbContext.Questions.FindAsync(questionId);
            if (q == null) throw new ObjectNotFoundException();
            this.Input = new InputModel {
                AnswerText = q.AnswerText,
                DisplayName = q.DisplayName,
                EmailAddress = q.EmailAddress,
                QuestionText = q.QuestionText,
                CategoryId = this.Categories.First().Value
            };
        }

        // Input model

        public class InputModel {
            [Required(ErrorMessage = "Není zadána otázka"), MaxLength(500), DataType(DataType.MultilineText)]
            public string QuestionText { get; set; }

            [DataType(DataType.MultilineText)]
            public string AnswerText { get; set; }

            [MaxLength(100)]
            public string DisplayName { get; set; }

            [MaxLength(100), DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
            public string EmailAddress { get; set; }

            public int CategoryId { get; set; }

        }

        public override async Task PreRender() {
            await base.PreRender();

            if (!this.Context.IsPostBack) await this.InitializeInputModel();
        }

        public async Task SaveChanges() {
            var questionId = Convert.ToInt32(this.Context.Parameters["questionId"]);
            var q = await this.dbContext.Questions.FindAsync(questionId);
            if (q == null) throw new ObjectNotFoundException();

            q.AnswerText = this.Input.AnswerText;
            q.CategoryId = this.Input.CategoryId;
            q.DateAnswered = DateTime.Now;
            q.DisplayName = this.Input.DisplayName;
            q.EmailAddress = this.Input.EmailAddress;
            q.QuestionText = this.Input.QuestionText;

            await this.dbContext.SaveChangesAsync();
            this.Context.RedirectToRoute("QuestionDetail");
        }
    }
}

