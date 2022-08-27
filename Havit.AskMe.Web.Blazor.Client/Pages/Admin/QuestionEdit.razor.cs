using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Components;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Shared.Contracts;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages.Admin {
	public class QuestionEditBase : PageBase {
		[Inject]
		public IQuestionClientFacade QuestionClientFacade { get; set; }

		[Inject]
		public ICategoryClientFacade CategoryClientFacade { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Parameter]
		public int QuestionId { get; set; }

		protected QuestionDto Model { get; set; }
		protected List<ListItemVM> Categories { get; set; }

		protected ServerSideValidator ServerSideValidator { get; set; }


		protected override async Task OnInitializedAsync() {
			this.Categories = await CategoryClientFacade.GetAll();
		}

		protected override async Task OnParametersSetAsync() {
			await base.OnParametersSetAsync();

			Model = await QuestionClientFacade.GetQuestionAsync(QuestionId);
		}

		public async Task HandleValidSubmit() {
			var result = await QuestionClientFacade.UpdateQuestionAsync(QuestionId, Model);
			if (result.Success) {
				NavigationManager.NavigateTo($"/Question/{QuestionId}");
			} else {
				foreach (var error in result.Errors) {
					ServerSideValidator.AddError(error);
				}
			}
		}
	}
}
