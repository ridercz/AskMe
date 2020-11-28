using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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

		public async Task<CollectionDataResult<List<QuestionVM>>> GetQuestionsAsync(QuestionListQueryFilter filter) {
			return await httpClient.GetFromJsonAsync<CollectionDataResult<List<QuestionVM>>>($"api/questions?pageIndex={filter.PageIndex}&answered={filter.Answered}");
		}

		public async Task<QuestionVM> GetQuestionAsync(int questionId) {
			return await httpClient.GetFromJsonAsync<QuestionVM>($"api/questions/{questionId}");
		}

		public async Task<int> CreateQuestionAsync(QuestionCreateIM inputModel) {
			var respose = await httpClient.PostAsJsonAsync("api/questions", inputModel);
			return await respose.Content.ReadFromJsonAsync<int>();
		}

		public async Task<QuestionUpdateVM> UpdateQuestionAsync(int questionId, QuestionDto inputModel) {
			var response = await httpClient.PutAsJsonAsync($"api/questions/{questionId}", inputModel);
			return await response.Content.ReadFromJsonAsync<QuestionUpdateVM>();
		}
	}
}
