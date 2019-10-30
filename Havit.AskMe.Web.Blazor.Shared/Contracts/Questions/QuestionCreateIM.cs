using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Questions
{
	public class QuestionCreateIM
	{
		[Required(ErrorMessage = "Není zadána otázka")]
		[MaxLength(500)]
		[DataType(DataType.MultilineText)]
		public string QuestionText { get; set; }

		[MaxLength(100)]
		public string DisplayName { get; set; }

		[MaxLength(100)]
		[DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
		public string EmailAddress { get; set; }

		public string CategoryId { get; set; } = "1"; // Blazor WIP: Microsoft.AspNetCore.Components.Forms.InputSelect`1[System.Int32] does not support the type 'System.Int32'.
	}
}
