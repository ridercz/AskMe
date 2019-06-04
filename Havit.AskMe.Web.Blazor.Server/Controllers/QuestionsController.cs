using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Havit.AskMe.Web.Blazor.Server.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly AskDbContext askDbContext;
        private readonly AppConfiguration appConfiguration;

        public QuestionsController(AskDbContext askDbContext, IOptionsSnapshot<AppConfiguration> optionsSnapshot)
        {
            this.askDbContext = askDbContext;
            this.appConfiguration = optionsSnapshot.Value;
        }

        [HttpGet("api/questions")]
        public async Task<CollectionDataResult<List<QuestionListItemVM>>> GetQuestions([FromQuery]QuestionListQueryFilter filter)
        {
            filter.PageSize = appConfiguration.PageSize; // forced value ;-)

            IQueryable<Question> query = askDbContext.Questions;

            if (filter.Answered.HasValue)
            {
                query = filter.Answered.Value ? query.Where(q => q.DateAnswered.HasValue) : query.Where(q => !q.DateAnswered.HasValue);
            }

            var count = await query.CountAsync();

            var data = await query
                    .Skip(filter.PageIndex * filter.PageSize)
                    .Take(filter.PageSize)
                    .Select(q => new QuestionListItemVM()
                    {
                        QuestionId = q.Id,
                        DisplayName = q.DisplayName,
                        QuestionText = q.QuestionText,
                        CategoryName = q.Category.Name,
                        DateCreated = q.DateCreated,
                        DateAnswered = q.DateAnswered,
                        AnswerText = q.AnswerText
                    })
                    .ToListAsync();


            return new CollectionDataResult<List<QuestionListItemVM>>(data, filter, count);
        }
    }
}
