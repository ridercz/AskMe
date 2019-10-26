using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;

namespace Havit.AskMe.Web.Blazor.Client.Services
{
	public interface IQuestionClientFacade
	{
		Task<int> CreateQuestionAsync(QuestionCreateIM inputModel);
		Task<Blazor.Shared.Contracts.Questions.QuestionDto> GetQuestionAsync(int questionId);
		Task<CollectionDataResult<List<QuestionListItemVM>>> GetQuestionsAsync(QuestionListQueryFilter filter);
		Task<Blazor.Shared.Contracts.Questions.QuestionUpdateVM> UpdateQuestionAsync(int questionId, Blazor.Shared.Contracts.Questions.QuestionDto inputModel);
	}
}