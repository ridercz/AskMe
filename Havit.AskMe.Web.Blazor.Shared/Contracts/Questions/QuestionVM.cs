using System;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Questions {
	public class QuestionVM : QuestionDto {
		public int QuestionId { get; set; }
		public string CategoryName { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime? DateAnswered { get; set; }
	}
}