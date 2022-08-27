using System.ComponentModel.DataAnnotations;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Questions {
	public class QuestionDto {
		[Required(ErrorMessage = "Není zadána otázka")]
		[MaxLength(500)]
		[DataType(DataType.MultilineText)]
		public string QuestionText { get; set; }

		[MaxLength(int.MaxValue)]
		public string AnswerText { get; set; }

		[MaxLength(100)]
		public string DisplayName { get; set; }

		[MaxLength(100)]
		[DataType(DataType.EmailAddress, ErrorMessage = "Nesprávný formát e-mailové adresy")]
		public string EmailAddress { get; set; }

		public string CategoryId { get; set; }
	}
}
