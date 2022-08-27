using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Components;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages {
	public class QuestionsBase : PageBase {
		[Inject]
		private ICategoryClientFacade CategoryClientFacade { get; set; }

		[Inject]
		private IQuestionClientFacade QuestionClientFacade { get; set; }

		[Inject]
		private NavigationManager NavigationManager { get; set; }

		[Parameter]
		public int PageIndex { get; set; } = 0;

		protected List<ListItemVM> categories;
		protected CollectionDataResult<List<QuestionVM>> questions;
		protected QuestionCreateIM newQuestionIM = new QuestionCreateIM();
		protected ElementReference submitInput;

		protected override async Task OnInitializedAsync() {
			this.categories = await CategoryClientFacade.GetAll();
			await LoadQuestions();
		}

		protected async Task PageChanging(PagerBase.PageChangingEventArgs args) {
			this.PageIndex = args.NewPageNumber - 1;
			await LoadQuestions();
		}

		private async Task LoadQuestions() {
			this.questions = await QuestionClientFacade.GetQuestionsAsync(new QuestionListQueryFilter() { PageIndex = this.PageIndex, Answered = false });
		}

		protected async Task HandleNewQuestionValidSubmit() {
			var questionId = await QuestionClientFacade.CreateQuestionAsync(newQuestionIM);

			newQuestionIM = new QuestionCreateIM(); // reset form
			await LoadQuestions();
			NavigationManager.NavigateTo($"/questions#q_{questionId}", forceLoad: true);
		}
	}
}
