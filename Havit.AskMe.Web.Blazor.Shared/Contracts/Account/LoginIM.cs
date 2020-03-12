using System.ComponentModel.DataAnnotations;

namespace Havit.AskMe.Web.Blazor.Shared.Contracts.Account {
	public class LoginIM {
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
	}
}
