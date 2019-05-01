using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Altairis.AskMe.Web.DotVVM.Dto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Web.DotVVM.ViewModels {
    public class QuestionViewModel : MasterPageViewModel {
        private readonly AskDbContext dbContext;

        public QuestionViewModel(IHostingEnvironment env, AskDbContext dbContext) : base(env) {
            this.dbContext = dbContext;
        }

        public override string PageTitle => "Detail otázky";

        public AnsweredQuestionDto Item { get; set; }

        public async override Task PreRender() {
            var questionId = Convert.ToInt32(this.Context.Parameters["questionId"]);
            var q = await this.dbContext.Questions
                .ProjectTo<AnsweredQuestionDto>()
                .SingleOrDefaultAsync(x => x.Id == questionId);
            if (q == null) throw new ObjectNotFoundException();
            this.Item = Mapper.Map<AnsweredQuestionDto>(q);
        }
    }
}

