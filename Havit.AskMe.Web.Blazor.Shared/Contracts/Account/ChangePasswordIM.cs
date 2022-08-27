using System.ComponentModel.DataAnnotations;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Account {
	public class ChangePasswordIM {
		[Required]
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MinLength(12)]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Compare("NewPassword")]
		public string ConfirmPassword { get; set; }
	}
}
