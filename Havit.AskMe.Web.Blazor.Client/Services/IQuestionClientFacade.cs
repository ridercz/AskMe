using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;

namespace Havit.AskMe.Web.Blazor.Client.Services {
	public interface IQuestionClientFacade {
		Task<int> CreateQuestionAsync(QuestionCreateIM inputModel);
		Task<QuestionVM> GetQuestionAsync(int questionId);
		Task<CollectionDataResult<List<QuestionVM>>> GetQuestionsAsync(QuestionListQueryFilter filter);
		Task<QuestionUpdateVM> UpdateQuestionAsync(int questionId, QuestionDto inputModel);
	}
}