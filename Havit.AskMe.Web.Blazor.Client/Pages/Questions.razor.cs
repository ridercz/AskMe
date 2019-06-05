using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Components;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Shared;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages
{
	public class QuestionsBase : ComponentBase
	{
		[Inject]
		private ICategoryClientFacade CategoryClientFacade { get; set; }

		[Inject]
		private IQuestionClientFacade QuestionClientFacade { get; set; }

		[Inject]
		private IUriHelper UriHelper { get; set; }

		[Parameter]
		protected int PageIndex { get; set; } = 0;

		protected List<ListItemVM> categories;
		protected CollectionDataResult<List<QuestionListItemVM>> questions;
		protected QuestionIM newQuestionIM = new QuestionIM();

		protected override async Task OnInitAsync()
		{
			await LoadQuestions();
		}

		protected async Task PageChanging(PagerBase.PageChangingEventArgs args)
		{
			this.PageIndex = args.NewPageNumber - 1;
			await LoadQuestions();
		}

		private async Task LoadQuestions()
		{
			this.categories = await CategoryClientFacade.GetAll();
			this.questions = await QuestionClientFacade.GetQuestionsAsync(new QuestionListQueryFilter() { PageIndex = this.PageIndex, Answered = false });
		}

		protected async Task HandleNewQuestionValidSubmit()
		{
			var questionId = await QuestionClientFacade.CreateQuestionAsync(newQuestionIM);
			await LoadQuestions();
			UriHelper.NavigateTo($"/questions#q_{questionId}", forceLoad: true);
		}
	}
}
