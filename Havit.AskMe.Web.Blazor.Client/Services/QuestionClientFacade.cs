using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Services {
	public class QuestionClientFacade : IQuestionClientFacade {
		private readonly HttpClient httpClient;

		public QuestionClientFacade(HttpClient httpClient) {
			this.httpClient = httpClient;
		}

		public Task<CollectionDataResult<List<QuestionVM>>> GetQuestionsAsync(QuestionListQueryFilter filter) {
			return httpClient.GetJsonAsync<CollectionDataResult<List<QuestionVM>>>($"api/questions?pageIndex={filter.PageIndex}&answered={filter.Answered}");
		}

		public Task<QuestionVM> GetQuestionAsync(int questionId) {
			return httpClient.GetJsonAsync<QuestionVM>($"api/questions/{questionId}");
		}

		public Task<int> CreateQuestionAsync(QuestionCreateIM inputModel) {
			return httpClient.PostJsonAsync<int>("api/questions", inputModel);
		}

		public Task<QuestionUpdateVM> UpdateQuestionAsync(int questionId, QuestionDto inputModel) {
			return httpClient.PutJsonAsync<QuestionUpdateVM>($"api/questions/{questionId}", inputModel);
		}
	}
}
