using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.AskMe.Web.Blazor.Client.Components
{
	// Credits: https://remibou.github.io/Using-the-Blazor-form-validation/
	public class ServerSideValidator : ComponentBase
	{
		private ValidationMessageStore _messageStore;

		[CascadingParameter]
		public EditContext CurrentEditContext { get; set; }

		protected override void OnInitialized()
		{
			base.OnInitialized();

			if (CurrentEditContext == null)
			{
				throw new InvalidOperationException($"{nameof(ServerSideValidator)} requires a cascading " +
					$"parameter of type {nameof(EditContext)}. For example, you can use {nameof(ServerSideValidator)} " +
					$"inside an {nameof(EditForm)}.");
			}

			_messageStore = new ValidationMessageStore(CurrentEditContext);
			CurrentEditContext.OnValidationRequested += (s, e) => _messageStore.Clear();
			CurrentEditContext.OnFieldChanged += (s, e) => _messageStore.Clear(e.FieldIdentifier);
		}

		public void AddErrors(Dictionary<string, List<string>> errors)
		{
			foreach (var err in errors)
			{
				_messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);
			}
			CurrentEditContext.NotifyValidationStateChanged();
		}

		public void AddError(string fieldName, string error)
		{
			_messageStore.Add(CurrentEditContext.Field(fieldName), error);
			CurrentEditContext.NotifyValidationStateChanged();
		}

		public void AddError(string error)
		{
			AddError(String.Empty, error);
		}
	}
}
