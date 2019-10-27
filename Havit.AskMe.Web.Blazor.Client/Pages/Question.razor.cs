using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.AskMe.Web.Blazor.Client.Services;
using Havit.AskMe.Web.Blazor.Shared.Contracts.Questions;
using Microsoft.AspNetCore.Components;

namespace Havit.AskMe.Web.Blazor.Client.Pages
{
    public class QuestionBase : PageBase
    {
		[Inject]
		public IQuestionClientFacade QuestionClientFacade { get; set; }

		[Parameter]
		public int QuestionId { get; set; }

		protected QuestionVM Question { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			this.Question = await QuestionClientFacade.GetQuestionAsync(QuestionId);
		}

	}
}
