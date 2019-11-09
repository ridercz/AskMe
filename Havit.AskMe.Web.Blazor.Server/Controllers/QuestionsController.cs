using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altairis.AskMe.Data;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Havit.AskMe.Web.Blazor.Server.Controllers {
	public class QuestionsController : Controller {
		private readonly AskDbContext askDbContext;
		private readonly AppConfiguration appConfiguration;

		public QuestionsController(AskDbContext askDbContext, IOptionsSnapshot<AppConfiguration> optionsSnapshot) {
			this.askDbContext = askDbContext;
			this.appConfiguration = optionsSnapshot.Value;
		}

		[HttpGet("api/questions")]
		public async Task<CollectionDataResult<List<QuestionVM>>> GetQuestions([FromQuery]QuestionListQueryFilter filter) {
			filter.PageSize = appConfiguration.PageSize; // forced value from configuration ;-)

			IQueryable<Question> query = askDbContext.Questions;

			if (filter.Answered.HasValue) {
				query = filter.Answered.Value ? query.Where(q => q.DateAnswered.HasValue) : query.Where(q => !q.DateAnswered.HasValue);
			}

			var count = await query.CountAsync();

			var data = await query
					.OrderByDescending(q => q.DateAnswered).ThenByDescending(q => q.DateCreated)
					.Skip(filter.PageIndex * filter.PageSize)
					.Take(filter.PageSize)
					.Select(q => new QuestionVM() {
						QuestionId = q.Id,
						DisplayName = q.DisplayName,
						EmailAddress = q.EmailAddress,
						QuestionText = q.QuestionText,
						CategoryId = q.CategoryId.ToString(),
						CategoryName = q.Category.Name,
						DateCreated = q.DateCreated,
						DateAnswered = q.DateAnswered,
						AnswerText = q.AnswerText,
					})
					.ToListAsync();


			return new CollectionDataResult<List<QuestionVM>>(data, filter, count);
		}

		[HttpGet("api/questions/{questionId:int}")]
		public async Task<ActionResult<QuestionVM>> GetQuestion(int questionId) {
			// Get question
			var question = await askDbContext.Questions.Where(q => q.Id == questionId).Select(q => new QuestionVM() {
				QuestionId = q.Id,
				DisplayName = q.DisplayName,
				EmailAddress = q.EmailAddress,
				QuestionText = q.QuestionText,
				CategoryId = q.CategoryId.ToString(),
				CategoryName = q.Category.Name,
				DateCreated = q.DateCreated,
				DateAnswered = q.DateAnswered,
				AnswerText = q.AnswerText,
			}).FirstOrDefaultAsync();

			if (question == null) {
				return NotFound();
			}

			// Prepare model
			return Ok(question);
		}

		[HttpPost("api/questions")]
		public async Task<int> CreateQuestion([FromBody]QuestionCreateIM inputModel) {
			if (!this.ModelState.IsValid) {
				throw new InvalidOperationException("Invalid model state.");
			}

			// Create and save question entity
			var question = new Question {
				QuestionText = inputModel.QuestionText,
				CategoryId = int.Parse(inputModel.CategoryId),
				DisplayName = inputModel.DisplayName,
				EmailAddress = inputModel.EmailAddress
			};
			await this.askDbContext.Questions.AddAsync(question);
			await this.askDbContext.SaveChangesAsync();

			return question.Id;
		}

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPut("api/questions/{questionId:int}")]
		public async Task<ActionResult<QuestionUpdateVM>> UpdateQuestion(int questionId, [FromBody]QuestionDto inputModel) {
			if (!this.ModelState.IsValid) {
				var result = new QuestionUpdateVM();
				result.Errors = this.ModelState.SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage).ToArray();
				return Ok(result);
			}

			// Get question
			var question = await askDbContext.Questions.FindAsync(questionId);
			if (question == null) {
				return NotFound();
			}

			// Update question
			question.CategoryId = int.Parse(inputModel.CategoryId);
			question.DisplayName = inputModel.DisplayName;
			question.EmailAddress = inputModel.EmailAddress;
			question.QuestionText = inputModel.QuestionText;

			if (string.IsNullOrWhiteSpace(inputModel.AnswerText)) {
				question.AnswerText = null;
				question.DateAnswered = null;
			} else {
				question.AnswerText = inputModel.AnswerText;
				if (!question.DateAnswered.HasValue) {
					question.DateAnswered = DateTime.Now;
				}
			}

			await askDbContext.SaveChangesAsync();

			return Ok(new QuestionUpdateVM() { Success = true });
		}
	}
}
