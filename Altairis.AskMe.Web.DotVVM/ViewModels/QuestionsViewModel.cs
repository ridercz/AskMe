using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.DotVVM.Dto;
using AutoMapper.QueryableExtensions;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class QuestionsViewModel : PagedViewModel<UnansweredQuestionDto> {
        private readonly AskDbContext dbContext;

        [Bind(Direction.ServerToClientFirstRequest)]
        public IEnumerable<SelectListItemDto> Categories => this.dbContext.Categories
            .OrderBy(c => c.Name)
            .Select(c => new SelectListItemDto { Text = c.Name, Value = c.Id });

        public InputModel Input { get; set; }

        private void InitializeInputModel() => this.Input = new InputModel {
            CategoryId = this.Categories.First().Value
        };

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

        public QuestionsViewModel(IHostingEnvironment env, IOptionsSnapshot<AppConfiguration> config, AskDbContext dbContext) : base(env, config) {
            this.dbContext = dbContext;
        }

        public override string PageTitle => "Nezodpovìzené otázky";

        protected override IQueryable<UnansweredQuestionDto> DataSource
            => this.dbContext.Questions
                .Where(x => !x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateCreated)
                .ProjectTo<UnansweredQuestionDto>();

        public async Task SubmitQuestion() {
            // Create and save question entity
            var nq = new Question {
                QuestionText = this.Input.QuestionText,
                CategoryId = this.Input.CategoryId,
                DisplayName = this.Input.DisplayName,
                EmailAddress = this.Input.EmailAddress
            };
            await this.dbContext.Questions.AddAsync(nq);
            await this.dbContext.SaveChangesAsync();
            this.InitializeInputModel();
        }

        public override async Task PreRender() {
            await base.PreRender();

            if (!this.Context.IsPostBack) this.InitializeInputModel();
        }
    }
}

