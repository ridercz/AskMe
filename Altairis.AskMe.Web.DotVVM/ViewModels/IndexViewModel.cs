using System.Linq;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.DotVVM.Dto;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class IndexViewModel : PagedViewModel<AnsweredQuestionDto> {
        private readonly AskDbContext dbContext;

        public IndexViewModel(IHostingEnvironment env, IOptionsSnapshot<AppConfiguration> config, AskDbContext dbContext) : base(env, config) {
            this.dbContext = dbContext;
        }

        public override string PageTitle => "AskMe";

        protected override IQueryable<AnsweredQuestionDto> DataSource
            => this.dbContext.Questions
                .Where(x => x.DateAnswered.HasValue)
                .OrderByDescending(x => x.DateAnswered)
                .ProjectTo<AnsweredQuestionDto>();
    }
}

